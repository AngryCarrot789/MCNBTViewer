using System;
using MCNBTViewer.Core.Services;
using MCNBTViewer.Core.Views.Dialogs.FilePicking;
using MCNBTViewer.Core.Views.Dialogs.Message;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;

namespace MCNBTViewer.Core {
    public static class CoreIoC {
        public static SimpleIoC Instance { get; } = new SimpleIoC();

        public static IDispatcher Dispatcher { get; set; }
        public static IClipboardService Clipboard { get; set; }
        public static IMessageDialogService MessageDialogs { get; set; }
        public static IFilePickDialogService FilePicker { get; set; }
        public static IUserInputDialogService UserInput { get; set; }

        public static Action<string> BroadcastShortcutChanged { get; set; }

        // get => Instance.Provide<IUserInputDialogService>();
        // set => Instance.Register(value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null"));
        public static Action<string> BroadcastShortcutActivity { get; set; }
    }
}