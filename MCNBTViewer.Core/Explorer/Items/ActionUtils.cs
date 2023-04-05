using System;
using MCNBTViewer.Core.Actions;
using Action = MCNBTViewer.Core.Actions.Action;

namespace MCNBTViewer.Core.Explorer.Items {
    public static class ActionUtils {
        public static void Register(ActionType type, Action action) {
            ActionManager.Instance.Register(GetId(type), action);
        }

        public static void Register<T>(ActionType type) where T : Action, new(){
            ActionManager.Instance.Register(GetId(type), new T());
        }

        public static string GetId(ActionType actionType) {
            switch (actionType) {
                case ActionType.NBTCopyName:    return "actions.nbt.copy.name";
                case ActionType.NBTCopyValue:   return "actions.nbt.copy.value";
                case ActionType.NBTEditGeneral: return "actions.nbt.edit-general";
                default: throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
        }
    }

    public enum ActionType {
        NBTCopyName,
        NBTCopyValue,
        NBTEditGeneral
    }
}