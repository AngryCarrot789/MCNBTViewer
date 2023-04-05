using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MCNBTViewer.Core.Actions;

namespace MCNBTViewer.DynUI.Menus {
    public class AdvancedActionMenuItem : AdvancedMenuItem {
        public static readonly DependencyProperty ActionIdProperty = DependencyProperty.Register("ActionId", typeof(string), typeof(AdvancedActionMenuItem), new PropertyMetadata(null));
        public static readonly DependencyProperty InvokeActionAfterCommandProperty = DependencyProperty.Register("InvokeActionAfterCommand", typeof(bool), typeof(AdvancedActionMenuItem), new PropertyMetadata(default(bool)));

        public string ActionId {
            get => (string) this.GetValue(ActionIdProperty);
            set => this.SetValue(ActionIdProperty, value);
        }

        public bool InvokeActionAfterCommand {
            get => (bool) this.GetValue(InvokeActionAfterCommandProperty);
            set => this.SetValue(InvokeActionAfterCommandProperty, value);
        }

        protected override void OnClick() {
            string id = this.ActionId;
            if (string.IsNullOrEmpty(id)) {
                base.OnClick();
                return;
            }

            if (this.InvokeActionAfterCommand) {
                base.OnClick();
                this.DispatchAction(id);
            }
            else {
                this.DispatchAction(id);
                base.OnClick();
            }
        }

        protected virtual void DispatchAction(string id) {
            this.Dispatcher.Invoke(() => {
                ActionManager.Instance.Execute(id, this.DataContext, false);
            }, DispatcherPriority.Render);
        }
    }
}