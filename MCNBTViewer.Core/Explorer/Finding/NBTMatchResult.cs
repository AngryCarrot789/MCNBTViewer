using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer.Finding {
    public class NBTMatchResultViewModel {
        public BaseNBTViewModel NBT { get; }

        public string FoundName { get; }

        public string FoundValue { get; }

        public ObservableCollection<TextRange> NameMatches { get; }

        public ObservableCollection<TextRange> ValueMatches { get; }

        public NBTMatchResultViewModel(BaseNBTViewModel nbt, string foundName, string foundValue, IEnumerable<Match> nameMatches, IEnumerable<Match> valueMatches) :
            this(nbt, foundName, foundValue, nameMatches.Select(x => new TextRange(x.Index, x.Length)), valueMatches.Select(x => new TextRange(x.Index, x.Length))){
        }

        public NBTMatchResultViewModel(BaseNBTViewModel nbt, string foundName, string foundValue, IEnumerable<TextRange> nameMatches, IEnumerable<TextRange> valueMatches) {
            this.NBT = nbt;
            this.FoundName = foundName;
            this.FoundValue = foundValue;
            this.NameMatches = new ObservableCollection<TextRange>(nameMatches);
            this.ValueMatches = new ObservableCollection<TextRange>(valueMatches);
        }
    }
}