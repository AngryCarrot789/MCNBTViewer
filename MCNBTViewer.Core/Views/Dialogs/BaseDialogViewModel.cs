namespace MCNBTViewer.Core.Views.Dialogs {
    public abstract class BaseDialogViewModel : BaseViewModel {
        public IDialog Dialog { get; set; }

        protected BaseDialogViewModel() {

        }
    }
}