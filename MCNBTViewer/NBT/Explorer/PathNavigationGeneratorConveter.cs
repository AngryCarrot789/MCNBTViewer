using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.NBT.Explorer {
    public class PathNavigationGeneratorConveter : IValueConverter {
        public Style SeparatorRunStyle { get; set; }
        public Style HyperlinkRunStyle { get; set; }
        public Style HyperlinkStyle { get; set; }

        public bool AcceptNBT { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is List<string> list)) {
                if (this.AcceptNBT && value is BaseNBTViewModel nbt) {
                    list = nbt.PathChain;
                }
                else if (value != DependencyProperty.UnsetValue) {
                    return new List<Inline>();
                }
                else {
                    return DependencyProperty.UnsetValue;
                }
            }

            List<Inline> inlines = new List<Inline>();
            using (List<string>.Enumerator enumerator = list.GetEnumerator()) {
                StringBuilder sb = new StringBuilder();
                if (enumerator.MoveNext()) {
                    inlines.Add(this.CreateHyperlink(enumerator.Current, sb.Append(enumerator.Current)));
                }

                while (enumerator.MoveNext()) {
                    inlines.Add(this.CreateSeparator("/"));
                    inlines.Add(this.CreateHyperlink(enumerator.Current, sb.Append('/').Append(enumerator.Current)));
                }
            }

            return inlines;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        private Run CreateSeparator(string text) {
            return this.SeparatorRunStyle != null ? new Run(text) {Style = this.SeparatorRunStyle} : new Run(text);
        }

        private Hyperlink CreateHyperlink(string text, StringBuilder accumulatedFullPath) {
            Run run = this.HyperlinkRunStyle != null ? new Run(text) {Style = this.HyperlinkRunStyle} : new Run(text);
            Hyperlink link = this.HyperlinkStyle != null ? new Hyperlink(run) {Style = this.HyperlinkStyle} : new Hyperlink(run);
            link.Tag = accumulatedFullPath.ToString();
            link.Click += this.Link_Click;
            return link;
        }

        private async void Link_Click(object sender, RoutedEventArgs e) {
            if (sender is Hyperlink link && link.Tag is string path) {
                await IoC.MainExplorer.NavigateToPath(path);
            }
        }
    }
}