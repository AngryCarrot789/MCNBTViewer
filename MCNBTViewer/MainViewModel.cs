using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Explorer.Items.Regions;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Regions;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        public ICommand AddEmptyDATTagCommand { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenFolderCommand { get; }

        public ICommand SaveAllDatFilesCommands { get; }

        public ICommand ShowFindViewCommand { get; }

        public ICommand EditShortcutsCommand { get; }

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
            this.AddEmptyDATTagCommand = new RelayCommand(() => {
                int count = this.Explorer.RootFiles.Count + 1;
                for (int i = count, end = count + 1000; i <= end; i++) {
                    string newName = "New Tag " + i;
                    if (this.Explorer.RootFiles.All(x => !(x is BaseNBTViewModel nbt) || nbt.Name != newName)) {
                        this.Explorer.AddChild(new NBTDataFileViewModel(newName));
                        return;
                    }
                }
            });
            this.OpenFileCommand = new RelayCommand(async () => await this.OpenFileAction());
            this.OpenFolderCommand = new AsyncRelayCommand(this.OpenFolderActionAsync);
            this.SaveAllDatFilesCommands = new RelayCommand(async () => {
                int count = this.Explorer.RootFiles.Count, i = 0;
                foreach (BaseViewModel file in this.Explorer.RootFiles) {
                    if (!(file is NBTDataFileViewModel dat)) {
                        continue;
                    }

                    try {
                        await dat.SaveToFileAction(false);
                    }
                    catch (Exception e) {
                        if ((i + 1) >= count) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", $"Failed to write compressed NBT to file at:\n{dat.FilePath}\n{e.Message}");
                            break;
                        }
                        else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to write NBT", $"Failed to write compressed NBT to file at:\n{dat.FilePath}\n{e.Message}\nDo you want to continue saving the rest of the files?")) {
                            break;
                        }
                    }

                    i++;
                }
            });

            this.ShowFindViewCommand = new AsyncRelayCommand(this.ShowFindViewAsync, () => true);
            this.EditShortcutsCommand = new RelayCommand(() => {
                IoC.ShortcutManagerDialog.ShowEditorDialog();
            }, () => !IoC.ShortcutManagerDialog.IsOpen);
        }

        public async Task ShowFindViewAsync() {
            await Task.Delay(1);
            IoC.FindViewService.ShowFindView();
        }

        private void ShowFindViewAction() {
            IoC.FindViewService.ShowFindView();
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

        public static bool IsDatFile(string extension) {
            switch (extension) {
                case ".dat":
                case ".nbt":
                case ".schematic":
                case ".dat_mcr":
                case ".dat_old":
                case ".bpt":
                case ".rc":
                    return true;
                default: return false;
            }
        }

        public static bool IsRegionFile(string extension) {
            switch (extension) {
                case ".mcr":
                case ".mca":
                    return true;
                default: return false;
            }
        }

        public async Task ParseFilesAction(string[] files) {
            for (int i = 0; i < files.Length; i++) {
                string filePath = files[i];
                string fileName = Path.GetFileName(filePath);
                BaseViewModel match = this.Explorer.RootFiles.FirstOrDefault(x => x is BaseNBTViewModel nbt && nbt.Name == fileName);
                if (match != null) {
                    if (match is NBTDataFileViewModel dat) {
                        if (!string.IsNullOrEmpty(dat.FilePath)) {
                            await IoC.MessageDialogs.ShowMessageAsync("Already added", $"A root DAT file with the name '{fileName}' already exists at path:\n{dat.FilePath}");
                        }
                        else {
                            await IoC.MessageDialogs.ShowMessageAsync("Already added", $"A root DAT file with the name '{fileName}' was already added");
                        }
                    }
                    else {
                        await IoC.MessageDialogs.ShowMessageAsync("Already added", $"A root file with the name '{fileName}' was already added");
                    }

                    continue;
                }

                string extension = Path.GetExtension(filePath);
                if (true) { // IsDatFile(extension)
                    NBTTagCompound compound;
                    try {
                        compound = CompressedStreamTools.Read(filePath, out _, IoC.UseCompression, IoC.IsBigEndian);
                    }
                    catch (Exception e) {
                        if ((i + 1) >= files.Length) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to read NBT", $"Failed to read NBT file at:\n{filePath}\n{e.Message}\n. Try turning on/off compression; the file may or may not be using compressed NBT (File->Use Compression)");
                            break;
                        }
                        else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to read NBT", $"Failed to read NBT file at:\n{filePath}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                            break;
                        }
                        else {
                            continue;
                        }
                    }

                    try {
                        this.AddChildToExplorer(new NBTDataFileViewModel(fileName, compound) {
                            FilePath = filePath
                        });
                    }
                    catch (Exception e) {
                        if ((i + 1) >= files.Length) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to parse NBT", $"Failed to parse NBT into UI elements for file at:\n{filePath}\n{e.Message}");
                        }
                        else if (await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to parse NBT", $"Failed to parse NBT into UI elements for file at:\n{filePath}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                            continue;
                        }

                        break;
                    }
                }
                else if (IsRegionFile(extension)) {
                    RegionFile file = new RegionFile(filePath);
                    try {
                        file.ReadFile();
                    }
                    catch (Exception e) {
                        if ((i + 1) >= files.Length) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to read region", $"Failed to read region file at:\n{filePath}\n{e.Message}");
                            break;
                        }
                        else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to read region", $"Failed to read region file at:\n{filePath}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                            break;
                        }
                        else {
                            continue;
                        }
                    }

                    try {
                        RegionItemViewModel region = new RegionItemViewModel();
                        region.ClearAndLoadChunks(file);
                        this.AddChildToExplorer(region);
                    }
                    catch (Exception e) {
                        if ((i + 1) >= files.Length) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to parse region", $"Failed to parse region file data at:\n{filePath}\n{e.Message}");
                            break;
                        }
                        else if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Failed to parse region", $"Failed to parse region file data at:\n{filePath}\n{e.Message}\nDo you want to continue reading the rest of the files?")) {
                            break;
                        }
                        else {
                            continue;
                        }
                    }
                }
                else if (files.Length == 1) {
                    await IoC.MessageDialogs.ShowMessageAsync("Unknown file type", $"Unknown file type: {extension}");
                }
            }
        }

        public void AddChildToExplorer(BaseViewModel file) {
            this.Explorer.AddChild(file);
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