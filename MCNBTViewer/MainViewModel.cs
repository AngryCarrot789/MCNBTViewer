using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public ICommand SaveAllDatFilesCommands { get; }

        public ICommand ShowFindViewCommand { get; }

        private bool isBigEndian;
        public bool IsBigEndian {
            get => this.isBigEndian;
            set => this.RaisePropertyChanged(ref this.isBigEndian, value, () => {
                IoC.IsBigEndian = this.IsBigEndian;
            });
        }

        private bool useCompression;
        public bool UseCompression {
            get => this.useCompression;
            set => this.RaisePropertyChanged(ref this.useCompression, value, () => {
                IoC.UseCompression = this.UseCompression;
            });
        }

        public MainViewModel() {
            this.IsBigEndian = true;
            this.UseCompression = true;
            this.Explorer = new NBTExplorerViewModel();
            IoC.MainExplorer = this.Explorer;
            this.OpenFileCommand = new RelayCommand(async () => await this.OpenFileAction());
            this.OpenFolderCommand = new AsyncRelayCommand(this.OpenFolderActionAsync);

            this.SaveAllDatFilesCommands = new RelayCommand(async () => {
                int count = this.Explorer.LoadedDataFiles.Count, i = 0;
                foreach (NBTDataFileViewModel file in this.Explorer.LoadedDataFiles) {
                    try {
                        if (File.Exists(file.FilePath)) {
                            file.SaveToFile();
                        }
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

            this.ShowFindViewCommand = new AsyncRelayCommand(this.ShowFindViewAsync, () => true);
        }

        public async Task ShowFindViewAsync() {
            await Task.Delay(1);
            IoC.FindView.ShowFindView();
        }

        private void ShowFindViewAction() {
            IoC.FindView.ShowFindView();
        }

        private async Task OpenFolderActionAsync() {
            DialogResult<string> result = IoC.FilePicker.ShowFolderPickerDialog(titleBar: "Select a folder to open");
            if (result.IsSuccess) {
                if (await IoC.MessageDialogs.ShowYesNoDialogAsync("Open all files?", "Do you want to open all files in all subdirectories in this folder? (otherwise, just this folder specifically)", false)) {
                    await this.ParseFilesAction(Directory.GetFiles(result.Value, "*.dat", SearchOption.AllDirectories));
                }
                else {
                    await this.ParseFilesAction(Directory.GetFiles(result.Value, "*.dat"));
                }
            }
        }

        public async Task OpenFileAction() {
            DialogResult<string[]> result = IoC.FilePicker.ShowFilePickerDialog(Filters.NBTDatAndAllFilesFilter, titleBar: "Select NBT files to open", multiSelect: true);
            if (result.IsSuccess) {
                await this.ParseFilesAction(result.Value);
            }
        }

        public async Task ParseFilesAction(string[] files) {
            for (int i = 0; i < files.Length; i++) {
                string fileName = Path.GetFileName(files[0]);
                NBTDataFileViewModel match = this.Explorer.LoadedDataFiles.FirstOrDefault(x => x.Name == fileName);
                if (match != null) {
                    await IoC.MessageDialogs.ShowMessageAsync("Already added", $"A root DAT file with the name '{fileName}'{(string.IsNullOrEmpty(match.FilePath) ? "" : $" already exists at path:\n{match.FilePath}")}");
                    continue;
                }

                NBTTagCompound compound;
                try {
                    compound = CompressedStreamTools.ReadCompressed(files[i], out _, IoC.UseCompression, IoC.IsBigEndian);
                }
                catch (Exception e) {
                    if ((i + 1) >= files.Length) {
                        await IoC.MessageDialogs.ShowMessageAsync("Failed to read NBT", $"Failed to read NBT file at:\n{files[i]}\n{e.Message}\n. Try turning on/off compression; the file may or may not be using compressed NBT (File->Use Compression)");
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
                    this.AddDataFile(new NBTDataFileViewModel(fileName, compound) {
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
            this.Explorer.AddDataFile(file);
        }

        public static NBTTagCompound CreateRoot() {
            NBTTagCompound root = new NBTTagCompound();
            NBTTagList list = new NBTTagList();
            list.tags.Add(new NBTTagByte(23));
            list.tags.Add(new NBTTagFloat(0.357f));
            list.tags.Add(new NBTTagString("Yes this list is technically invalid :>"));
            list.tags.Add(new NBTTagString("NBTTagList can only contain 1 specific type of tag"));
            root.Put("listA", list);

            NBTTagCompound inner = new NBTTagCompound();
            NBTTagCompound inner2 = new NBTTagCompound();
            inner2.Put("lol", new NBTTagString("lololol"));
            inner2.Put("234y57894t", new NBTTagInt());
            inner.Put("inner2", inner2);
            root.Put("inner", inner);
            return root;
        }
    }
}