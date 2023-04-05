using System.Collections.Generic;

namespace MCNBTViewer.Core.Actions {
    public class AnActionEvent {
        private readonly Dictionary<string, object> data;

        public bool IsModal { get; }

        public AnActionEvent(Dictionary<string, object> data, bool modal) {
            this.data = data;
            this.IsModal = modal;
        }

        public T GetData<T>(string key) {
            return this.data.TryGetValue(key, out object obj) && obj is T value ? value : default;
        }

        public bool TryGetData<T>(string key, out T value) {
            if (this.data.TryGetValue(key, out object obj) && obj is T t) {
                value = t;
                return true;
            }

            value = default;
            return false;
        }
    }
}