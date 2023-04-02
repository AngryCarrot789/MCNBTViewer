using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Timing;
using MCNBTViewer.Core.Views.Windows;

namespace MCNBTViewer.Core.Explorer.Finding {
    public class FindViewModel : BaseWindowViewModel, IDisposable {
        public ICommand CloseCommand { get; }

        private bool isNameRegex;
        public bool IsNameRegex {
            get => this.isNameRegex;
            set {
                this.RaisePropertyChangedIfChanged(ref this.isNameRegex, value);
                if (value && this.IsNameSearchingWholeWord) {
                    this.IsNameSearchingWholeWord = false;
                }
            }
        }

        private bool isNameSearchingWholeWord;
        public bool IsNameSearchingWholeWord {
            get => this.isNameSearchingWholeWord;
            set {
                this.RaisePropertyChangedIfChanged(ref this.isNameSearchingWholeWord, value);
                if (value && this.IsNameRegex) {
                    this.IsNameRegex = false;
                }
            }
        }

        private bool isNameCaseSensitive;
        public bool IsNameCaseSensitive {
            get => this.isNameCaseSensitive;
            set => this.RaisePropertyChangedIfChanged(ref this.isNameCaseSensitive, value);
        }

        private bool includeCollectionNameMatches;
        public bool IncludeCollectionNameMatches {
            get => this.includeCollectionNameMatches;
            set => this.RaisePropertyChangedIfChanged(ref this.includeCollectionNameMatches, value);
        }

        private bool isValueRegex;
        public bool IsValueRegex {
            get => this.isValueRegex;
            set {
                this.RaisePropertyChangedIfChanged(ref this.isValueRegex, value);
                if (value && this.IsValueSearchingWholeWord) {
                    this.IsValueSearchingWholeWord = false;
                }
            }
        }

        private bool isValueSearchingWholeWord;
        public bool IsValueSearchingWholeWord {
            get => this.isValueSearchingWholeWord;
            set {
                this.RaisePropertyChangedIfChanged(ref this.isValueSearchingWholeWord, value);
                if (value && this.IsValueRegex) {
                    this.IsValueRegex = false;
                }
            }
        }

        private bool isValueCaseSentsitive;
        public bool IsValueCaseSentsitive {
            get => this.isValueCaseSentsitive;
            set => this.RaisePropertyChangedIfChanged(ref this.isValueCaseSentsitive, value);
        }

        private string searchForNameText;
        public string SearchForNameText {
            get => this.searchForNameText;
            set {
                this.RaisePropertyChanged(ref this.searchForNameText, value);
                this.OnInputChanged();
            }
        }

        private string searchForValueText;
        public string SearchForValueText {
            get => this.searchForValueText;
            set {
                this.RaisePropertyChanged(ref this.searchForValueText, value);
                this.OnInputChanged();
            }
        }

        private bool isSearchTermEmpty;
        public bool IsSearchTermEmpty {
            get => this.isSearchTermEmpty;
            set => this.RaisePropertyChanged(ref this.isSearchTermEmpty, value);
        }

        private bool isSearchActive;
        public bool IsSearchActive {
            get => this.isSearchActive;
            set => this.RaisePropertyChanged(ref this.isSearchActive, value);
        }

        public ObservableCollection<NBTMatchResult> FoundItems { get; }

        public IdleEventService IdleEventService { get; }

        private volatile bool stopTask;
        private Task searchTask;

        private readonly LinkedList<NBTMatchResult> queuedItemsToAdd;

        private volatile int runId;
        private volatile bool canProcessQueueTaskRun;
        private readonly Task processQueueTask;

        public FindViewModel() {
            this.isSearchTermEmpty = true;
            this.isSearchActive = false;
            this.CloseCommand = new RelayCommand(this.CloseDialogAction);
            this.FoundItems = new ObservableCollection<NBTMatchResult>();
            this.IdleEventService = new IdleEventService();
            this.IdleEventService.MinimumTimeSinceInput = TimeSpan.FromMilliseconds(200);
            this.IdleEventService.OnIdle += this.OnTickSearch;
            this.queuedItemsToAdd = new LinkedList<NBTMatchResult>();
            this.canProcessQueueTaskRun = true;
            this.processQueueTask = Task.Run(async () => {
                while (this.canProcessQueueTaskRun) {
                    if (this.queuedItemsToAdd.Count > 0) {
                        lock (this.queuedItemsToAdd) {
                            int runTaskId = this.runId;
                            int max = Math.Min(10, this.queuedItemsToAdd.Count);
                            List<NBTMatchResult> results = new List<NBTMatchResult>();
                            for (int i = 0; i < max; i++) {
                                LinkedListNode<NBTMatchResult> first = this.queuedItemsToAdd.First;
                                this.queuedItemsToAdd.RemoveFirst();
                                results.Add(first.Value);
                            }

                            IoC.Dispatcher.Invoke(() => {
                                if (runTaskId == this.runId) {
                                    foreach (NBTMatchResult result in results) {
                                        this.FoundItems.Add(result);
                                    }
                                }
                            });
                        }
                    }

                    await Task.Delay(100);
                }
            });
        }

        public void StopActiveSearch(bool clearResults) {
            this.StopTaskAndWait(clearResults);
            this.IsSearchActive = false;
            if (clearResults) {
                this.FoundItems.Clear();
            }
        }

        private void OnInputChanged() {
            if (string.IsNullOrEmpty(this.SearchForNameText) && string.IsNullOrEmpty(this.searchForValueText)) {
                this.StopTaskAndWait(true);
                this.FoundItems.Clear();
                this.IdleEventService.CanFireNextTick = false;
                this.IsSearchTermEmpty = true;
                this.IsSearchActive = false;
            }
            else {
                this.IsSearchTermEmpty = false;
                this.IdleEventService.OnInput();
            }
        }

        private void StopTaskAndWait(bool clearItemQueue = true) {
            if (this.searchTask != null) {
                this.stopTask = true;
                this.searchTask.Wait();
            }

            if (clearItemQueue) {
                lock (this.queuedItemsToAdd) {
                    this.queuedItemsToAdd.Clear();
                }
            }
        }

        public void OnTickSearch() {
            this.StopTaskAndWait();
            if (string.IsNullOrEmpty(this.SearchForNameText) && string.IsNullOrEmpty(this.SearchForValueText)) {
                this.IsSearchTermEmpty = true;
                this.IsSearchActive = false;
                return;
            }

            Interlocked.Increment(ref this.runId);
            lock (this.queuedItemsToAdd) {
                this.queuedItemsToAdd.Clear();
                this.FoundItems.Clear();
            }

            FindFlags nf = FindFlags.None;
            if (this.isNameRegex) nf |= FindFlags.Regex;
            if (this.isNameSearchingWholeWord) nf |= FindFlags.Words;
            if (this.isNameCaseSensitive) nf |= FindFlags.Cases;

            FindFlags vf = FindFlags.None;
            if (this.isValueRegex) vf |= FindFlags.Regex;
            if (this.isValueSearchingWholeWord) vf |= FindFlags.Words;
            if (this.isValueCaseSentsitive) vf |= FindFlags.Cases;

            this.stopTask = false;
            string searchName = string.IsNullOrEmpty(this.SearchForNameText) ? null : this.SearchForNameText;
            string searchValue = string.IsNullOrEmpty(this.SearchForValueText) ? null : this.SearchForValueText;
            this.IsSearchActive = true;
            this.searchTask = Task.Run(() => this.TaskMain(searchName, searchValue, nf, vf));
        }

        // nf = name flags, vf = value flags
        private async Task TaskMain(string searchName, string searchValue, FindFlags nf, FindFlags vf) {
            // cheap way of avoid concurrent modification in most cases
            List<NBTDataFileViewModel> list = IoC.MainExplorer.LoadedDataFiles.ToList();
            foreach (NBTDataFileViewModel file in list) {
                if (this.stopTask) {
                    return;
                }

                await this.FindItems(file, file.Children.ToList(), searchName, searchValue, nf, vf);
            }

            this.IsSearchActive = false;
        }

        private async Task FindItems(BaseNBTCollectionViewModel parent, List<BaseNBTViewModel> items, string searchName, string searchValue, FindFlags nf, FindFlags vf) {
            foreach (BaseNBTViewModel child in items) {
                List<TextRange> nameMatchesNormal = new List<TextRange>();
                List<TextRange> valueMatchesNormal = new List<TextRange>();
                if (this.stopTask) {
                    return;
                }

                if (searchName != null) {
                    if (child is BaseNBTCollectionViewModel childCollection) {
                        if (!string.IsNullOrEmpty(child.Name) && this.includeCollectionNameMatches) {
                            List<TextRange> collectionMatches = new List<TextRange>();
                            if (AcceptName(searchName, child, nf, collectionMatches)) {
                                lock (this.queuedItemsToAdd) {
                                    this.queuedItemsToAdd.AddLast(new NBTMatchResult(child, searchName, null, null, collectionMatches, null));
                                }
                            }
                        }

                        await this.FindItems(childCollection, childCollection.Children.ToList(), searchName, searchValue, nf, vf);
                        continue;
                    }
                    else if (string.IsNullOrEmpty(child.Name) || !AcceptName(searchName, child, nf, nameMatchesNormal)) {
                        continue;
                    }
                }

                string foundValue = null;
                if (searchValue != null) {
                    if (child is NBTPrimitiveViewModel || child is BaseNBTArrayViewModel) {
                        if (!AcceptValue(searchValue, child, vf, valueMatchesNormal, out foundValue)) {
                            continue;
                        }
                    }
                    else if (child is BaseNBTCollectionViewModel childCollection) {
                        await this.FindItems(childCollection, childCollection.Children.ToList(), searchName, searchValue, nf, vf);
                        continue;
                    }
                    else {
                        continue;
                    }
                }

                if (this.stopTask) {
                    return;
                }

                if (nameMatchesNormal.Count > 0 || valueMatchesNormal.Count > 0) {
                    lock (this.queuedItemsToAdd) {
                        this.queuedItemsToAdd.AddLast(new NBTMatchResult(child, searchName, searchValue, foundValue, nameMatchesNormal, valueMatchesNormal));
                    }
                }
            }
        }

        public void CloseDialogAction() {
            this.Window.CloseWindow();
        }

        public void Dispose() {
            this.IdleEventService?.Dispose();
            this.canProcessQueueTaskRun = false;
            this.processQueueTask.Wait();
            this.StopTaskAndWait(true);
        }

        private static bool AcceptName(string pattern, BaseNBTViewModel nbt, FindFlags flags, List<TextRange> matches) {
            if (flags.IsRegex()) {
                if (!AcceptValueRegex(pattern, nbt.Name, flags.IsIgnoreCase(), matches)) {
                    return false;
                }
            }
            else {
                if (!AcceptValue(pattern, nbt.Name, flags.IsIgnoreCase(), flags.IsWords(), matches)) {
                    return false;
                }
            }

            return true;
        }

        private static bool AcceptValue(string pattern, BaseNBTViewModel nbt, FindFlags flags, List<TextRange> matches, out string foundValue) {
            if (nbt is NBTPrimitiveViewModel) {
                foundValue = ((NBTPrimitiveViewModel) nbt).Data ?? "";
            }
            else if (nbt is NBTIntArrayViewModel) {
                int[] data = ((NBTIntArrayViewModel) nbt).Data;
                foundValue = data != null && data.Length > 0 ? string.Join(",", data) : "";
            }
            else if (nbt is NBTByteArrayViewModel) {
                byte[] data = ((NBTByteArrayViewModel) nbt).Data;
                foundValue = data != null && data.Length > 0 ? string.Join(",", data) : "";
            }
            else {
                foundValue = null;
                return false;
            }

            if (flags.IsRegex()) {
                if (!AcceptValueRegex(pattern, foundValue, flags.IsIgnoreCase(), matches)) {
                    return false;
                }
            }
            else {
                if (!AcceptValue(pattern, foundValue, flags.IsIgnoreCase(), flags.IsWords(), matches)) {
                    return false;
                }
            }

            return true;
        }

        private static bool AcceptValueRegex(string pattern, string value, bool ignoreCase, List<TextRange> matches) {
            MatchCollection collection = Regex.Matches(value, pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
            if (collection.Count < 1) {
                return false;
            }

            foreach (Match match in collection) {
                matches.Add(new TextRange(match.Index, match.Length));
            }

            return true;
        }

        private static bool AcceptValue(string pattern, string value, bool ignoreCase, bool words, List<TextRange> matches) {
            int len = pattern.Length, i = -len;
            StringComparison compType = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            while ((i = value.IndexOf(pattern, i + len, compType)) != -1) {
                int endIndex = i + len;
                if (!words || ((i == 0 || value[i - 1] == ' ') && (endIndex >= value.Length || value[endIndex] == ' '))) {
                    matches.Add(new TextRange(i, len));
                }
            }

            return matches.Count > 0;
        }
    }
}