using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.NBT.Controls {
    public class NBTBaseControl : Control {
        public NBTBase NBT { get; set; }

        /// <summary>
        /// A simple way to implicitly cast the NBT data, because c# is silly and doesn't
        /// have the feature that java has had since like way before 2010
        /// </summary>
        public T GetNBT<T>() where T : NBTBase {
            return (T) this.NBT;
        }
    }
}