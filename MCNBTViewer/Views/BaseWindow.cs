using System.Threading.Tasks;
using MCNBTViewer.Core.Views.Windows;

namespace MCNBTViewer.Views {
    public class BaseWindow : BaseWindowCore, IWindow {
        public void CloseWindow() {
            this.Close();
        }

        public async Task CloseWindowAsync() {
            await this.Dispatcher.InvokeAsync(this.CloseWindow);
        }
    }
}