using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTDataFileViewModel : NBTCompoundViewModel, IContextProvider {
        private string filePath;
        public string FilePath {
            get => this.filePath;
            set => this.RaisePropertyChanged(ref this.filePath, value);
        }

        private bool hasFileSavedOnce;

        public AsyncRelayCommand RefreshDatFileCommand { get; }

        public AsyncRelayCommand ShowInExplorerCommand { get; }

        public AsyncRelayCommand RemoveDatFileFromTreeCommand { get; }

        public AsyncRelayCommand DeleteDatFileCommand { get; }
        public AsyncRelayCommand CopyFilePathToClipboardCommand { get; }

        public AsyncRelayCommand SaveDatFileCommand { get; }
        public AsyncRelayCommand SaveDatFileAsCommand { get; }


        public NBTDataFileViewModel(BaseNBTCollectionViewModel nbt, bool deepCopy = false) : this(nbt.Name) {
            foreach (BaseNBTViewModel pair in nbt.Children) {
                this.Children.Add(deepCopy ? CreateFrom(pair.Name, pair.ToNBT()) : pair);
            }
        }

        public NBTDataFileViewModel(string name, NBTTagCompound nbt) : this(name) {
            foreach (KeyValuePair<string, NBTBase> pair in nbt.map) {
                this.Children.Add(CreateFrom(pair.Key, pair.Value));
            }
        }

        public NBTDataFileViewModel(string name) : base(name) {
            this.RefreshDatFileCommand = new AsyncRelayCommand(this.RefreshAction, () => File.Exists(this.FilePath));
            this.RemoveDatFileFromTreeCommand = new AsyncRelayCommand(this.RemoveSelfAction, () => IoC.MainExplorer != null && IoC.MainExplorer.LoadedDataFiles.Contains(this));
            this.DeleteDatFileCommand = new AsyncRelayCommand(this.DeleteFileAction, () => File.Exists(this.FilePath));
            this.ShowInExplorerCommand = new AsyncRelayCommand(this.OpenInExplorerAction, () => File.Exists(this.FilePath) && IoC.ExplorerService != null);
            this.CopyFilePathToClipboardCommand = new AsyncRelayCommand(async () => {
                if (!string.IsNullOrEmpty(this.FilePath)) {
                    if (IoC.Clipboard != null) {
                        IoC.Clipboard.ReadableText = this.FilePath;
                    }
                    else {
                        await IoC.MessageDialogs.ShowMessageAsync("No clipboard", "Clipboard is unavailable. File path: " + this.FilePath);
                    }
                }
            }, () => IoC.Clipboard != null && !string.IsNullOrEmpty(this.FilePath));
            this.SaveDatFileCommand = new AsyncRelayCommand(() => this.SaveToFileAction(false));
            this.SaveDatFileAsCommand = new AsyncRelayCommand(() => this.SaveToFileAction(true));
        }

        private async Task RefreshAction() {
            if (File.Exists(this.FilePath)) {
                NBTTagCompound compound;
                try {
                    compound = CompressedStreamTools.ReadCompressed(this.FilePath, out _, IoC.UseCompression, IoC.IsBigEndian);
                }
                catch (Exception e) {
                    await IoC.MessageDialogs.ShowMessageAsync("Failed to refresh NBT", $"Failed to read NBT file at:\n{this.FilePath}\n{e.Message}");
                    return;
                }

                try {
                    foreach (KeyValuePair<string, NBTBase> pair in compound.map) {
                        this.Children.Add(CreateFrom(pair.Key, pair.Value));
                    }
                }
                catch (Exception e) {
                    await IoC.MessageDialogs.ShowMessageAsync("Failed to parse NBT", $"Failed to parse NBT into UI elements for file at:\n{this.FilePath}\n{e.Message}");
                }
            }
        }

        private async Task RemoveSelfAction() {
            if (IoC.MainExplorer != null) {
                IoC.MainExplorer.RemoveDatFile(this);
            }
        }

        private async Task DeleteFileAction() {
            bool canRemove = false;
            if (File.Exists(this.FilePath)) {
                try {
                    File.Delete(this.FilePath);
                    canRemove = true;
                }
                catch (Exception e) {
                    await IoC.MessageDialogs.ShowMessageAsync("Failed to delete file", $"Failed to delete {this.filePath}:\n{e.Message}");
                }
            }

            if (canRemove && IoC.MainExplorer != null && await IoC.MessageDialogs.ShowYesNoDialogAsync("Remove dat file?", "Do you want to also remove the DAT file from the list?")) {
                IoC.MainExplorer.RemoveDatFile(this);
            }
        }

        private async Task OpenInExplorerAction() {
            if (IoC.ExplorerService != null) {
                IoC.ExplorerService.OpenFileInExplorer(this.FilePath);
            }
        }

        public void SaveToFile() {
            this.SaveToFile(this.FilePath);
        }

        public void SaveToFile(string filePath) {
            CompressedStreamTools.WriteCompressed(this.ToNBT(), filePath, IoC.UseCompression, IoC.IsBigEndian);
        }

        public async Task SaveToFileAction(bool saveAs) {
            string path;
            if (saveAs || string.IsNullOrEmpty(this.FilePath) || (!this.hasFileSavedOnce && !File.Exists(this.FilePath))) {
                DialogResult<string> result = IoC.FilePicker.ShowSaveFileDialog(Filters.NBTDatAndAllFilesFilter, titleBar: "Select a save location for the NBT file");
                if (!result.IsSuccess || string.IsNullOrEmpty(result.Value)) {
                    return;
                }

                path = result.Value;
            }
            else {
                path = this.FilePath;
            }

            string name = System.IO.Path.GetFileName(path);
            NBTDataFileViewModel alreadyExists = IoC.MainExplorer.LoadedDataFiles.FirstOrDefault(x => x.Name == name);
            if (alreadyExists != null && alreadyExists != this) {
                if (!await IoC.MessageDialogs.ShowYesNoDialogAsync("Same name already in tree", $"A DAT file with the name '{name}' already exists in the tree. Continue and remove the existing DAT tag from the tree? (otherwise, don't load the new file)")) {
                    return;
                }
            }

            try {
                this.SaveToFile(path);
                this.hasFileSavedOnce = true;
                if (!path.Equals(this.FilePath)) {
                    this.FilePath = path;
                }

                this.Name = name;
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", $"Failed to write compressed NBT to file at:\n{this.FilePath}\n{e.Message}");
            }
        }

        public override IEnumerable<IBaseContextEntry> GetContextEntries() {
            // this.RefreshDatFileCommand.RaiseCanExecuteChanged();
            // this.RemoveDatFileFromTreeCommand.RaiseCanExecuteChanged();
            // this.DeleteDatFileCommand.RaiseCanExecuteChanged();
            // this.ShowInExplorerCommand.RaiseCanExecuteChanged();
            // this.CopyFilePathToClipboardCommand.RaiseCanExecuteChanged();
            yield return new ContextEntry("New", null, this.GetNewItemsEntries());
            yield return ContextEntrySeparator.Instance;
            foreach (IBaseContextEntry entry in this.GetSortingContextEntries()) {
                yield return entry;
            }

            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Refresh from file", this.RefreshDatFileCommand) {
                ToolTip = File.Exists(this.FilePath) ? "Reload all of the NBT data from the underlying file" : "File does not exist"
            };
            yield return new ContextEntry("Save", this.SaveDatFileCommand) {
                ToolTip = "Saves this NBT data to a file. If the underlying file does not exist, a new dialog is shown"
            };
            yield return new ContextEntry("Save As...", this.SaveDatFileAsCommand) {
                ToolTip = File.Exists(this.FilePath) ? "Reload all of the NBT data from the underlying file" : "File does not exist"
            };
            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Show in explorer", this.ShowInExplorerCommand);
            yield return new ContextEntry("Copy File Path", this.CopyFilePathToClipboardCommand);
            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Copy (Binary)", this.CopyBinaryToClipboardCommand);
            yield return new ContextEntry("Paste (Binary)", this.PasteNBTBinaryDataCommand);
            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Remove this root tag", this.RemoveDatFileFromTreeCommand) {
                ToolTip = "Removes this root DAT file tag from the tree. Does not delete the file"
            };
            yield return new ContextEntry("Delete FILE", this.DeleteDatFileCommand) {
                ToolTip = File.Exists(this.FilePath) ? "Deletes the DAT file from your computer" : "File was already deleted"
            };
        }
    }
}