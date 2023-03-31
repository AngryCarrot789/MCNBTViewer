using System;

namespace MCNBTViewer.Core.VFS.Data {
    // Syntax aid
    public class VFKey<T> : VFKey {
        public Type Type { get; }

        public VFKey(string name) : base(name) {
            this.Type = typeof(T);
        }
    }

    public abstract class VFKey {
        public string Name { get; }

        protected VFKey(string name) {
            this.Name = name;
        }
    }
}