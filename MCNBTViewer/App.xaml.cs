using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MCNBTViewer.Converters;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Services;
using MCNBTViewer.Core.Shortcuts.Managing;
using MCNBTViewer.Core.Shortcuts.ViewModels;
using MCNBTViewer.NBT.Explorer.Dialogs;
using MCNBTViewer.NBT.Explorer.Finding;
using MCNBTViewer.Services;
using MCNBTViewer.Shortcuts;
using MCNBTViewer.Shortcuts.Dialogs;
using MCNBTViewer.Shortcuts.Views;
using MCNBTViewer.Views.Dialogs.FilePicking;
using MCNBTViewer.Views.Dialogs.Message;
using MCNBTViewer.Views.Dialogs.UserInputs;

namespace MCNBTViewer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private static void UpdateShortcutResourcesRecursive(ResourceDictionary dictionary, ShortcutGroup group) {
            foreach (ShortcutGroup innerGroup in group.Groups) {
                UpdateShortcutResourcesRecursive(dictionary, innerGroup);
            }

            foreach (ManagedShortcut shortcut in group.Shortcuts) {
                UpdatePath(dictionary, shortcut.Path);
            }
        }

        private static void UpdatePath(ResourceDictionary dictionary, string shortcut) {
            string resourcePath = "ShortcutPaths." + shortcut;
            if (dictionary.Contains(resourcePath)) {
                dictionary[resourcePath] = ShortcutPathToInputGestureTextConverter.ShortcutToInputGestureText(shortcut);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e) {
            IoC.MessageDialogs = new MessageDialogService();
            IoC.Dispatcher = new DispatcherDelegate(this);
            IoC.Clipboard = new ClipboardService();
            IoC.FilePicker = new FilePickDialogService();
            IoC.UserInput = new UserInputDialogService();
            IoC.ExplorerService = new WinExplorerService();
            IoC.TagDialogService = new NBTDialogService();
            IoC.FindViewService = new FindViewService();
            InputStrokeViewModel.KeyToReadableString = KeyStrokeRepresentationConverter.ToStringFunction;
            InputStrokeViewModel.MouseToReadableString = MouseStrokeRepresentationConverter.ToStringFunction;
            IoC.KeyboardDialogs = new KeyboardDialogService();
            IoC.MouseDialogs = new MouseDialogService();
            IoC.ShortcutManager = AppShortcutManager.Instance;
            IoC.ShortcutManagerDialog = new ShortcutManagerDialogService();
            IoC.OnShortcutManagedChanged = (x) => {
                if (!string.IsNullOrWhiteSpace(x)) {
                    ShortcutGestureConverter.BroadcastChange();
                    // UpdatePath(this.Resources, x);
                }
            };

            // pack://application:,,,/MCNBTViewer;component/Keymap.xml
            // Uri uri = new Uri("pack://application:,,,/Keymap.xml");
            // new BitmapImage(uri);
            // Uri otherUri = PackUriHelper.GetPackageUri(uri);
            string filePath = @"F:\VSProjsV2\MCNBTViewer\MCNBTViewer\Keymap.xml";
            if (File.Exists(filePath)) {
                AppShortcutManager.Instance.Root = null;
                using (FileStream stream = File.OpenRead(filePath)) {
                    ShortcutGroup group = WPFKeyMapDeserialiser.Instance.Deserialise(stream);
                    AppShortcutManager.Instance.Root = group;
                }
            }
            else {
                MessageBox.Show("Keymap file does not exist: " + filePath);
            }

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();

            if (this.MainWindow.DataContext is MainViewModel view) {
                // const string debugPath = "C:\\Users\\kettl\\Desktop\\TheRareCarrot.dat";
                // if (File.Exists(debugPath)) {
                //     NBTTagCompound compound = CompressedStreamTools.ReadCompressed(debugPath, out _);
                //     view.AddDataFile(new NBTDataFileViewModel(Path.GetFileName(debugPath), compound) { FilePath = debugPath });
                // }

                view.AddChildToExplorer(new NBTDataFileViewModel("Demo Tag", MainViewModel.CreateRoot()));
            }
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
