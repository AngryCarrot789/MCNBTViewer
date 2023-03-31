using System;

namespace MCNBTViewer.Core.VFS.Data {
    // Syntax aid
    public class FKey<T> : FKey {
        public Type Type { get; }

        public FKey(string name) : base(name) {
            this.Type = typeof(T);
        }
    }

    public abstract class FKey {
        public string Name { get; }

        protected FKey(string name) {
            this.Name = name;
        }
    }
}