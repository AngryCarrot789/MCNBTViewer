using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCNBTViewer.Core.VFS.Data {
    public class VFDataHolderMap {
        private readonly Dictionary<VFKey, object> map;

        public int Count => this.map.Count;

        public bool IsEmpty => this.map.Count < 1;

        public ICollection<VFKey> Keys => this.map.Keys;

        public ICollection<object> Values => this.map.Values;

        public object this[VFKey key] {
            get => this.map.TryGetValue(key, out object value) ? value : null;
            set => this.map[key] = value;
        }

        public VFDataHolderMap() {
            this.map = new Dictionary<VFKey, object>();
        }

        public T Get<T>(VFKey<T> key) {
            return this.map.TryGetValue(key, out object value) ? (T) value : default;
        }

        public bool TryGet<T>(VFKey<T> key, out T value) {
            if (this.map.TryGetValue(key, out object obj)) {
                value = (T) obj;
                return true;
            }

            value = default;
            return false;
        }

        public void Put<T>(VFKey<T> key, T value) {
            this.map[key] = value;
        }

        public T Replace<T>(VFKey<T> key, T value) {
            T replaced = this.map.TryGetValue(key, out object old) ? (T) old : default;
            this.map[key] = value;
            return replaced;
        }
    }
}