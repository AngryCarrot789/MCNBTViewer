using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.AdvancedContextService.Base;
using MCNBTViewer.Core.Explorer.Items;

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
            this.NameMatches = nameMatches ?? new List<TextRange>();
            this.ValueMatches = valueMatches ?? new List<TextRange>();
            this.NavigateToItemCommand = new AsyncRelayCommand(this.NavigateToItemAction);
        }

        private async Task NavigateToItemAction() {
            await IoC.MainExplorer.TreeView.Behaviour.RepeatExpandHierarchyFromRootAsync(this.NBT.ParentChain, true);
        }

        public List<IContextEntry> GetContext(List<IContextEntry> list) {
            list.Add(new CommandContextEntry("Navigate", this.NavigateToItemCommand));
            list.Add(ContextEntrySeparator.Instance);
            return this.NBT.GetContext(list);
        }
    }
}