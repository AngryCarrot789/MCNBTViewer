using System.Windows;

namespace MCNBTViewer.Views.Dialogs.UserInputs {
    /// <summary>
    /// Interaction logic for DoubleUserInputWindow.xaml
    /// </summary>
    public partial class DoubleUserInputWindow : BaseDialog {
        public SingleInputValidationRule InputValidationRuleA => this.Resources["ValidatorInputA"] as SingleInputValidationRule;
        public SingleInputValidationRule InputValidationRuleB => this.Resources["ValidatorInputB"] as SingleInputValidationRule;

        public DoubleUserInputWindow() {
            this.InitializeComponent();
            this.Loaded += this.WindowOnLoaded;
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e) {
            this.InputBoxA.Focus();
            this.InputBoxA.SelectAll();
        }
    }
}
