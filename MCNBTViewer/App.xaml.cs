using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Services;
using MCNBTViewer.NBT.Explorer.Dialogs;
using MCNBTViewer.NBT.Explorer.Finding;
using MCNBTViewer.Services;
using MCNBTViewer.Views.Dialogs.FilePicking;
using MCNBTViewer.Views.Dialogs.Message;
using MCNBTViewer.Views.Dialogs.UserInputs;

namespace MCNBTViewer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            IoC.MessageDialogs = new MessageDialogService();
            IoC.Dispatcher = new DispatcherDelegate(this);
            IoC.Clipboard = new ClipboardService();
            IoC.FilePicker = new FilePickDialogService();
            IoC.UserInput = new UserInputDialogService();
            IoC.ExplorerService = new WinExplorerService();
            IoC.TagDialogService = new NewTagDialogService();
            IoC.FindView = new FindView();

            Task.Run(() => {

            });

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();

            // if (this.MainWindow.DataContext is MainViewModel view) {
            //     const string debugPath = "C:\\Users\\kettl\\Desktop\\TheRareCarrot.dat";
            //     if (File.Exists(debugPath)) {
            //         NBTTagCompound compound = CompressedStreamTools.ReadCompressed(debugPath, out _);
            //         view.AddDataFile(new NBTDataFileViewModel(Path.GetFileName(debugPath), compound) { FilePath = debugPath });
            //     }
            // }
        }

        private class DispatcherDelegate : IDispatcher {
            private readonly App app;

            public DispatcherDelegate(App app) {
                this.app = app;
            }

            public void InvokeLater(Action action) {
                this.app.Dispatcher.Invoke(action, DispatcherPriority.Normal);
            }

            public void Invoke(Action action) {
                this.app.Dispatcher.Invoke(action);
            }

            public T Invoke<T>(Func<T> function) {
                return this.app.Dispatcher.Invoke(function);
            }

            public async Task InvokeAsync(Action action) {
                await this.app.Dispatcher.InvokeAsync(action);
            }

            public async Task<T> InvokeAsync<T>(Func<T> function) {
                return await this.app.Dispatcher.InvokeAsync(function);
            }
        }
    }
}
