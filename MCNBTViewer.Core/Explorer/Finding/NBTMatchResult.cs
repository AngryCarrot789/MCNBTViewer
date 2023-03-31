using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.Finding {
    public class NBTMatchResult : IContextProvider {
        public BaseNBTViewModel NBT { get; }

        public string NameSearchTerm { get; }

        public string ValueSearchTerm { get; }

        public string PrimitiveOrArrayFoundValue { get; }

        public List<TextRange> NameMatches { get; }

        public List<TextRange> ValueMatches { get; }

        public ICommand NavigateToItemCommand { get; }

        public NBTMatchResult(BaseNBTViewModel nbt, string nameSearchTerm, string valueSearchTerm, string primitiveOrArrayFoundValue, IEnumerable<Match> nameMatches, IEnumerable<Match> valueMatches) :
            this(nbt, nameSearchTerm, valueSearchTerm, primitiveOrArrayFoundValue, nameMatches.Select(x => new TextRange(x.Index, x.Length)), valueMatches.Select(x => new TextRange(x.Index, x.Length))){
        }

        public NBTMatchResult(BaseNBTViewModel nbt, string nameSearchTerm, string valueSearchTerm, string primitiveOrArrayFoundValue, IEnumerable<TextRange> nameMatches, IEnumerable<TextRange> valueMatches) :
            this(nbt, nameSearchTerm, valueSearchTerm, primitiveOrArrayFoundValue, nameMatches != null ? nameMatches.ToList() : new List<TextRange>(), valueMatches != null ? valueMatches.ToList() : new List<TextRange>()){
        }

        public NBTMatchResult(BaseNBTViewModel nbt, string nameSearchTerm, string valueSearchTerm, string primitiveOrArrayFoundValue, List<TextRange> nameMatches, List<TextRange> valueMatches) {
            this.NBT = nbt;
            this.NameSearchTerm = nameSearchTerm;
            this.ValueSearchTerm = valueSearchTerm;
            this.PrimitiveOrArrayFoundValue = primitiveOrArrayFoundValue;
            this.NameMatches = nameMatches;
            this.ValueMatches = valueMatches;
            this.NavigateToItemCommand = new RelayCommand(this.NavigateToItemAction);
        }

        private void NavigateToItemAction() {
            IoC.MainExplorer.SetSelectedItem(this.NBT);
        }

        public IEnumerable<IBaseContextEntry> GetContextEntries() {
            // if (!string.IsNullOrEmpty(this.PrimitiveOrArrayFoundValue)) {
            //     yield return new LazyASFContextEntry("Copy value", async () => {
            //         await ClipboardUtils.SetClipboardOrShowErrorDialog(this.PrimitiveOrArrayFoundValue);
            //     });
            //     yield return ContextEntrySeparator.Instance;
            // }

            if (this.NBT is IContextProvider provider) {
                foreach (IBaseContextEntry entry in provider.GetContextEntries()) {
                    yield return entry;
                }
            }
        }
    }
}