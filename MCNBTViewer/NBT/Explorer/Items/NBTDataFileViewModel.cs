namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTDataFileViewModel : NBTCompoundViewModel {
        private string fileFilePath;
        public string FilePath {
            get => this.fileFilePath;
            set => this.RaisePropertyChanged(ref this.fileFilePath, value);
        }

        public NBTDataFileViewModel() {
            
        }
    }
}