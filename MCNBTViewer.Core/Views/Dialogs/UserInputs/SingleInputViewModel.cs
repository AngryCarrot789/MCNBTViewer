using System.Collections.Generic;
using MCNBTViewer.Core.Views.ViewModels;

namespace MCNBTViewer.Core.Views.Dialogs.UserInputs {
    public class MySingleInputViewModel : BaseConfirmableDialogViewModel, IErrorInfoHandler {
        private string title;
        public string Title {
            get => this.title;
            set => this.RaisePropertyChanged(ref this.title, value);
        }

        private string message;
        public string Message {
            get => this.message;
            set => this.RaisePropertyChanged(ref this.message, value);
        }

        private string input;
        public string Input {
            get => this.input;
            set => this.RaisePropertyChanged(ref this.input, value);
        }

        public InputValidator ValidateInput { get; set; }

        public MySingleInputViewModel(IDialog dialog) : base(dialog) {

        }

        public void OnErrorsUpdated(Dictionary<string, object> errors) {
            this.ConfirmCommand.IsEnabled = errors.Count < 1;
        }
    }
}
