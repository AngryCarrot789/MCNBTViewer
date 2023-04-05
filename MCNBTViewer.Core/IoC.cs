using System;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Dialogs;
using MCNBTViewer.Core.Explorer.Finding;
using MCNBTViewer.Core.Services;
using MCNBTViewer.Core.Views.Dialogs.FilePicking;
using MCNBTViewer.Core.Views.Dialogs.Message;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;

namespace MCNBTViewer.Core {
    public static class IoC {
        public static SimpleIoC Instance { get; } = new SimpleIoC();

        public static IDispatcher Dispatcher { get; set; }
        public static IClipboardService Clipboard { get; set; }
        public static IMessageDialogService MessageDialogs { get; set; }
        public static IFilePickDialogService FilePicker { get; set; }
        public static IUserInputDialogService UserInput { get; set; }
        public static IExplorerService ExplorerService { get; set; }
        public static INBTDialogService TagDialogService { get; set; }

        public static NBTExplorerViewModel MainExplorer { get; set; }

        public static IFindViewService FindViewService { get; set; }

        public static bool IsBigEndian { get; set; }
        public static bool UseCompression { get; set; }

        public static Action<string> BroadcastShortcutActivity { get; set; }
    }
}