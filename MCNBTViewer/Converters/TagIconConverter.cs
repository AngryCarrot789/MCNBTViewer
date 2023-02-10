﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.Converters {
    public class TagIconConverter : IValueConverter {
        public Uri TagEnd { get; set; }
        public Uri TagByte { get; set; }
        public Uri TagShort { get; set; }
        public Uri TagInt { get; set; }
        public Uri TagLong { get; set; }
        public Uri TagFloat { get; set; }
        public Uri TagDouble { get; set; }
        public Uri TagString { get; set; }
        public Uri TagByteArray { get; set; }
        public Uri TagIntArray { get; set; }

        public Uri TagList { get; set; }

        public Uri TagCompoundClosed { get; set; }

        public Uri TagCompoundOpenEmpty { get; set; }

        public Uri TagCompoundOpenFull { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is NBTType type) {
                switch (type) {
                    case NBTType.End: return this.TagEnd;
                    case NBTType.Byte: return this.TagByte;
                    case NBTType.Short: return this.TagShort;
                    case NBTType.Int: return this.TagInt;
                    case NBTType.Long: return this.TagLong;
                    case NBTType.Float: return this.TagFloat;
                    case NBTType.Double: return this.TagDouble;
                    case NBTType.ByteArray: return this.TagByteArray;
                    case NBTType.String: return this.TagString;
                    case NBTType.IntArray: return this.TagIntArray;
                    case NBTType.List: return this.TagList;
                    case NBTType.Compound: {
                        return this.TagCompoundClosed;
                    }
                }

                throw new Exception($"Cannot convert NBTType {value}");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}