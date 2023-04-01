using System;
using System.Collections.Generic;
using System.Linq;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Dialogs;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    public class NBTDialogService : INBTDialogService {
        public (string, string)? CreatePrimitiveOrArrayTag(int tagType, bool canEditName = true, string titlebar = "Create a new tag", string initialName = null, string initialValue = null) {
            switch (tagType) {
                case 9:
                case 10:
                    throw new Exception("Non-primitive tag type: " + ((NBTType) tagType));
            }

            InputValidator validator = InputValidator.FromFunc((a) => {
                switch (tagType) {
                    case 1: return byte.TryParse(a, out _) ? null : $"Invalid byte value: {a}";
                    case 2: return short.TryParse(a, out _) ? null : $"Invalid short value: {a}";
                    case 3: return int.TryParse(a, out _) ? null : $"Invalid int value: {a}";
                    case 4: return long.TryParse(a, out _) ? null : $"Invalid long value: {a}";
                    case 5: return float.TryParse(a, out _) ? null : $"Invalid float value: {a}";
                    case 6: return double.TryParse(a, out _) ? null : $"Invalid double value: {a}";
                    case 8: return null;
                    case 7: return (a ?? "").Split(',').All(x => byte.TryParse(x, out _)) ? null : $"Invalid byte array: {a}\nThe input must be byte values separated by commas";
                    case 11: return (a ?? "").Split(',').All(x => int.TryParse(x, out _)) ? null : $"Invalid int array: {a}\nThe input must be int values separated by commas";
                    default: throw new Exception("Non-primitive tag type: " + ((NBTType) tagType));
                }
            });

            if (canEditName) {
                DoubleInputViewModel vm = new DoubleInputViewModel() {
                    MessageA = "Input the Tag name:",
                    InputA = initialName,
                    MessageB = "Input the Tag's value:",
                    InputB = initialValue,
                    ValidateInputA = Validators.ForNonEmptyString("Tag name cannot be an empty string"),
                    ValidateInputB = validator,
                    Title = titlebar
                };

                if (IoC.UserInput.ShowDoubleInputDialog(vm)) {
                    return (vm.InputA, vm.InputB);
                }
                else {
                    return null;
                }
            }
            else {
                SingleInputViewModel vm = new SingleInputViewModel() {
                    Message = "Input the Tag's value:",
                    Input = initialValue,
                    ValidateInput = validator,
                    Title = titlebar
                };

                if (IoC.UserInput.ShowSingleInputDialog(vm)) {
                    return (null, vm.Input);
                }
                else {
                    return null;
                }
            }
        }

        public (string, NBTTagByte)? CreateTagByte(bool canEditName, string defaultName, NBTTagByte defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Byte, canEditName, "byte tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myByte" : defaultName) : null, defaultTag?.data.ToString() ?? "0");
            return result.HasValue && byte.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagByte)?) (result.Value.Item1, new NBTTagByte(val)) : null;
        }

        public (string, NBTTagShort)? CreateTagShort(bool canEditName, string defaultName, NBTTagShort defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Short, canEditName, "short tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myShort" : defaultName) : null, defaultTag?.data.ToString() ?? "0");
            return result.HasValue && short.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagShort)?) (result.Value.Item1, new NBTTagShort(val)) : null;
        }

        public (string, NBTTagInt)? CreateTagInt(bool canEditName, string defaultName, NBTTagInt defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Int, canEditName, "int tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myInt" : defaultName) : null, defaultTag?.data.ToString() ?? "0");
            return result.HasValue && int.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagInt)?) (result.Value.Item1, new NBTTagInt(val)) : null;
        }

        public (string, NBTTagLong)? CreateTagLong(bool canEditName, string defaultName, NBTTagLong defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Long, canEditName, "long tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myLong" : defaultName) : null, defaultTag?.data.ToString() ?? "0");
            return result.HasValue && long.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagLong)?) (result.Value.Item1, new NBTTagLong(val)) : null;
        }

        public (string, NBTTagFloat)? CreateTagFloat(bool canEditName, string defaultName, NBTTagFloat defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Float, canEditName, "float tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myFloat" : defaultName) : null, defaultTag?.data.ToString() ?? "0f");
            return result.HasValue && float.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagFloat)?) (result.Value.Item1, new NBTTagFloat(val)) : null;
        }

        public (string, NBTTagDouble)? CreateTagDouble(bool canEditName, string defaultName, NBTTagDouble defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.Double, canEditName, "double tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myDouble" : defaultName) : null, defaultTag?.data.ToString() ?? "0d");
            return result.HasValue && double.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagDouble)?) (result.Value.Item1, new NBTTagDouble(val)) : null;
        }

        public (string, NBTTagByteArray)? CreateTagByteArray(bool canEditName, string defaultName, NBTTagByteArray defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.ByteArray, canEditName, "byte array tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myByteArray" : defaultName) : null, defaultTag != null && defaultTag.data != null ? ArrayToString(defaultTag.data) : "0,1,2");
            return result.HasValue && TryParseAll<byte>(byte.TryParse, result.Value.Item2, out var val) ? ((string, NBTTagByteArray)?) (result.Value.Item1, new NBTTagByteArray(val)) : null;
        }

        public (string, NBTTagString)? CreateTagString(bool canEditName, string defaultName, NBTTagString defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.String, canEditName, "string tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myString" : defaultName) : null, defaultTag?.data ?? "value here");
            return result.HasValue ? ((string, NBTTagString)?) (result.Value.Item1, new NBTTagString(result.Value.Item2)) : null;
        }

        public (string, NBTTagList)? CreateTagList(bool canEditName, string defaultName, NBTTagList defaultTag) {
            SingleInputViewModel vm = new SingleInputViewModel() {
                Message = "Input the Tag's name:",
                Input = defaultName ?? "myList",
                ValidateInput = Validators.ForNonEmptyString("Tag name cannot be an empty string"),
                Title = "Create a new NBTTagList"
            };

            if (IoC.UserInput.ShowSingleInputDialog(vm)) {
                return (vm.Input, new NBTTagList());
            }
            else {
                return null;
            }
        }

        public (string, NBTTagCompound)? CreateTagCompound(bool canEditName, string defaultName, NBTTagCompound defaultTag) {
            SingleInputViewModel vm = new SingleInputViewModel() {
                Message = "Input the Tag's name:",
                Input = defaultName ?? "myList",
                ValidateInput = Validators.ForNonEmptyString("Tag name cannot be an empty string"),
                Title = "NBTTagList"
            };

            if (IoC.UserInput.ShowSingleInputDialog(vm)) {
                return (vm.Input, new NBTTagCompound());
            }
            else {
                return null;
            }
        }

        public (string, NBTTagIntArray)? CreateTagIntArray(bool canEditName, string defaultName, NBTTagIntArray defaultTag) {
            var result = this.CreatePrimitiveOrArrayTag((int) NBTType.IntArray, canEditName, "int array tag", canEditName ? (string.IsNullOrEmpty(defaultName) ? "myIntArray" : defaultName) : null, defaultTag != null && defaultTag.data != null ? ArrayToString(defaultTag.data) : "0,1,2");
            return result.HasValue && TryParseAll<int>(int.TryParse, result.Value.Item2, out var val) ? ((string, NBTTagIntArray)?) (result.Value.Item1, new NBTTagIntArray(val)) : null;
        }

        private static string ArrayToString<T>(IEnumerable<T> array) {
            return string.Join(",", array);
        }

        private delegate bool ParseFunction<T>(string input, out T value);

        private static bool TryParseAll<T>(ParseFunction<T> parse, string input, out T[] values) {
            List<T> list = new List<T>();
            foreach (string str in input.Split(',')) {
                if (parse(str, out T value)) {
                    list.Add(value);
                }
                else {
                    values = null;
                    return false;
                }
            }

            values = list.ToArray();
            return true;
        }
    }
}