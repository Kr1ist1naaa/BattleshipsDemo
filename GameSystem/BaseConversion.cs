using System.Collections.Generic;
using System.Linq;
using Domain.DomainRule;

namespace GameSystem {
    public static class BaseConversion {
        private static readonly Dictionary<int, string> ConversionDictionary = new Dictionary<int, string>();

        public static string MapToBase26(int index) {
            if (!ConversionDictionary.Keys.Contains(index)) {
                return null;
            }

            return ConversionDictionary.FirstOrDefault(m => m.Key == index).Value;
        }

        public static int? MapToBase10(string index) {
            index = index.ToUpper();
            
            if (!ConversionDictionary.Values.Contains(index)) {
                return null;
            }
            
            return ConversionDictionary.FirstOrDefault(m => m.Value == index).Key;
        }

        public static void Reset() {
            ConversionDictionary.Clear();

            for (int i = 0; i < Rules.GetVal(RuleType.BoardSize); i++) {
                ConversionDictionary.Add(i, ToBase26(i + 1));
            }
        }

        private static string ToBase26(int number) {
            var list = new LinkedList<int>();
            list.AddFirst((number - 1) % 26);

            while ((number = --number / 26 - 1) > 0) {
                list.AddFirst(number % 26);
            }

            return new string(list.Select(s => (char) (s + 65)).ToArray());
        }
    }
}