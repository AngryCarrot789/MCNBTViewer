using System;
using System.Collections.Generic;
using System.Globalization;

namespace MCNBTViewer.Core.DynUI.Actions {
    public class ActionManager {
        public static ActionManager Instance { get; }

        private readonly Dictionary<string, AnAction> actions;

        public ActionManager() {
            this.actions = new Dictionary<string, AnAction>();
        }

        static ActionManager() {
            Instance = new ActionManager();
        }

        public void Register(string id, AnAction action) {
            this.actions[id] = action;
        }

        public AnAction GetAction(string id) {
            return this.actions.TryGetValue(id, out AnAction action) ? action : null;
        }

        public bool Execute(string id, object dataContext, bool isCalledModally) {
            if (this.actions.TryGetValue(id, out AnAction action)) {
                this.Execute(action, dataContext, isCalledModally);
                return true;
            }

            return false;
        }

        public void Execute(AnAction action, object dataContext, bool isCalledModally) {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["DataContext"] = dataContext;
            AnActionEvent e = new AnActionEvent(dict, isCalledModally);
            this.Execute(action, e);
        }

        public void Execute(AnAction action, AnActionEvent e) {
            if (action == null) {
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            }

            action.Execute(e);
        }
    }
}