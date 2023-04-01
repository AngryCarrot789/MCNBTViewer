using System;
using System.Collections.Generic;
using System.Linq;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Dialogs;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    public class InbtDialogService : INBTDialogService {
        public (string, string)? CreatePrimitiveTag(int tagType, bool canEditName = true, string titlebar = "Create a new tag compound", string initialName = null, string initialValue = null) {
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
                    default: throw new Exception("Non-primitive tag type: " + tagType);
                }
            });

            if (canEditName) {
                DoubleInputViewModel vm = new DoubleInputViewModel() {
                    MessageA = "Input the Tag name:",
                    InputA = initialName,
                    MessageB = "Input the Tag's value:",
                    InputB = initialValue,
                    ValidateInputA = InputValidator.FromFunc((x) => string.IsNullOrEmpty(x) ? "Tag name cannot be an empty string" : null),
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

        public (string, NBTTagByte)? CreateTagByte(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new byte tag", canEditName ? "myByte" : null, "0");
            return result.HasValue && byte.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagByte)?) (result.Value.Item1, new NBTTagByte(val)) : null;
        }

        public (string, NBTTagShort)? CreateTagShort(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new short tag", canEditName ? "myShort" : null, "0");
            return result.HasValue && short.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagShort)?) (result.Value.Item1, new NBTTagShort(val)) : null;
        }

        public (string, NBTTagInt)? CreateTagInt(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new int tag", canEditName ? "myInt" : null, "0");
            return result.HasValue && int.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagInt)?) (result.Value.Item1, new NBTTagInt(val)) : null;
        }

        public (string, NBTTagLong)? CreateTagLong(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new long tag", canEditName ? "myLong" : null, "0");
            return result.HasValue && long.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagLong)?) (result.Value.Item1, new NBTTagLong(val)) : null;
        }

        public (string, NBTTagFloat)? CreateTagFloat(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new float tag", canEditName ? "myFloat" : null, "0f");
            return result.HasValue && float.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagFloat)?) (result.Value.Item1, new NBTTagFloat(val)) : null;
        }

        public (string, NBTTagDouble)? CreateTagDouble(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new double tag", canEditName ? "myDouble" : null, "0d");
            return result.HasValue && double.TryParse(result.Value.Item2, out var val) ? ((string, NBTTagDouble)?) (result.Value.Item1, new NBTTagDouble(val)) : null;
        }

        public (string, NBTTagByteArray)? CreateTagByteArray(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new byte array tag", canEditName ? "myByteArray" : null, "0,1,2");
            return result.HasValue && TryParseAll<byte>(byte.TryParse, result.Value.Item2, out var val) ? ((string, NBTTagByteArray)?) (result.Value.Item1, new NBTTagByteArray(val)) : null;
        }

        public (string, NBTTagString)? CreateTagString(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new string tag", canEditName ? "myString" : null, "value here");
            return result.HasValue ? ((string, NBTTagString)?) (result.Value.Item1, new NBTTagString(result.Value.Item2)) : null;
        }

        public (string, NBTTagList)? CreateTagList(bool canEditName = true) {
            throw new NotImplementedException();
        }

        public (string, NBTTagCompound)? CreateTagCompound(bool canEditName = true) {
            throw new NotImplementedException();
        }

        public (string, NBTTagIntArray)? CreateTagIntArray(bool canEditName = true) {
            var result = this.CreatePrimitiveTag(1, canEditName, "Create a new int array tag", canEditName ? "myIntArray" : null, "0,1,2");
            return result.HasValue && TryParseAll<int>(int.TryParse, result.Value.Item2, out var val) ? ((string, NBTTagIntArray)?) (result.Value.Item1, new NBTTagIntArray(val)) : null;
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