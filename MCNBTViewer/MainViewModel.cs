using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FramePFX.Core;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs;
using MCNBTViewer.Core.Views.Dialogs.FilePicking;
using BaseViewModel = REghZy.MVVM.ViewModels.BaseViewModel;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public ICommand SaveAllDatFilesCommands { get; }

        public MainViewModel() {
            this.Explorer = new NBTExplorerViewModel();
            IoC.MainExplorer = this.Explorer;
            this.OpenFileCommand = new RelayCommand(async () => await this.OpenFileAction());
            this.OpenFolderCommand = new RelayCommand(async () => {
                await this.OpenFolderAction();
            });

            this.SaveAllDatFilesCommands = new RelayCommand(async () => {
                int count = this.Explorer.LoadedDataFiles.Count, i = 0;
                foreach (NBTDataFileViewModel file in this.Explorer.LoadedDataFiles) {
                    try {
                        file.SaveToFile();
                    }
                    catch (Exception e) {
                        if ((i + 1) >= count) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", $"Failed to write compressed NBT to file at:\n{file.FilePath}\n{e.Message}");
                            break;
                        }
                        else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to write NBT", $"Failed to write compressed NBT to file at:\n{file.FilePath}\n{e.Message}\nDo you want to continue saving the rest of the files?")) {
                            break;
                        }
                    }

                    i++;
                }
            });
        }

        private async Task OpenFolderAction() {
            DialogResult<string> result = IoC.FilePicker.ShowFolderPickerDialog(titleBar: "Select a folder to open");
            if (result.IsSuccess) {
                if (await IoC.MessageDialogs.ShowYesNoDialogAsync("Open all files?", "Do you want to open all files deep in this folder (all subdirectories), or just this folder specifically?")) {
                    await this.ParseFilesAction(CollectAllFiles(result.Value));
                }
                else {
                    await this.ParseFilesAction(Directory.GetFiles(result.Value, "*.dat"));
                }
            }
        }

        private static string[] CollectAllFiles(string folder) {
            return Directory.GetFiles(folder, "*.dat", SearchOption.AllDirectories);
        }

        public async Task OpenFileAction() {
            DialogResult<string[]> result = IoC.FilePicker.ShowFilePickerDialog(Filters.NBTDatAndAllFilesFilter, titleBar: "Select NBT files to open", multiSelect: true);
            if (result.IsSuccess) {
                await this.ParseFilesAction(result.Value);
            }
        }

        public async Task ParseFilesAction(string[] files) {
            for (int i = 0; i < files.Length; i++) {
                NBTTagCompound compound;
                try {
                    compound = CompressedStreamTools.ReadCompressed(files[i], out _);
                }
                catch (Exception e) {
                    if ((i + 1) >= files.Length) {
                        await IoC.MessageDialogs.ShowMessageAsync("Failed to read NBT", $"Failed to read NBT file at:\n{files[i]}\n{e.Message}");
                        break;
                    }
                    else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to read NBT", $"Failed to read NBT file at:\n{files[i]}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                        break;
                    }
                    else {
                        continue;
                    }
                }

                try {
                    this.AddDataFile(new NBTDataFileViewModel(Path.GetFileName(files[0]), compound) {
                        FilePath = files[0]
                    });
                }
                catch (Exception e) {
                    if ((i + 1) >= files.Length) {
                        await IoC.MessageDialogs.ShowMessageAsync("Failed to parse NBT", $"Failed to parse NBT into UI elements for file at:\n{files[i]}\n{e.Message}");
                    }
                    else if (await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to parse NBT", $"Failed to parse NBT into UI elements for file at:\n{files[i]}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                        continue;
                    }

                    break;
                }
            }
        }

        public void AddDataFile(NBTDataFileViewModel file) {
            this.Explorer.LoadedDataFiles.Add(file);
        }

        public static NBTCompoundViewModel CreateRoot() {
            NBTTagCompound root = new NBTTagCompound();
            NBTTagList list = new NBTTagList();
            list.tags.Add(new NBTTagByte(23));
            list.tags.Add(new NBTTagFloat(0.357f));
            list.tags.Add(new NBTTagString("23"));
            root.Put("listA", list);

            NBTTagCompound inner = new NBTTagCompound();
            NBTTagCompound inner2 = new NBTTagCompound();
            inner2.Put("lol", new NBTTagString("lololol"));
            inner2.Put("234y57894t", new NBTTagInt());
            inner.Put("inner2", inner2);
            root.Put("inner", inner);
            return BaseNBTViewModel.CreateFrom(null, root);
        }
    }
}