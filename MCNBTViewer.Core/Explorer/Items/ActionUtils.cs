using System;
using MCNBTViewer.Core.Actions;
using Action = MCNBTViewer.Core.Actions.Action;

namespace MCNBTViewer.Core.Explorer.Items {
    public static class NBTActions {
        public static void Register(NBTAction type, Action action) {
            ActionManager.Instance.Register(GetId(type), action);
        }

        public static void Register<T>(NBTAction type) where T : Action, new(){
            ActionManager.Instance.Register(GetId(type), new T());
        }

        public static string GetId(NBTAction action) {
            switch (action) {
                case NBTAction.CopyName:    return "actions.nbt.copy.name";
                case NBTAction.CopyValue:   return "actions.nbt.copy.value";
                case NBTAction.EditGeneral: return "actions.nbt.edit-general";
                default: throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public enum NBTAction {
        CopyName,
        CopyValue,
        EditGeneral
    }
}