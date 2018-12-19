using System.Collections.Generic;
using System.Linq;
using Domain.DomainRule;

namespace GameSystem {
    public static class BaseConversion {
        private static readonly Dictionary<int, string> ConversionDictionary = new Dictionary<int, string>();

        static BaseConversion() {
            // Create entries according to max board size rule
            for (int i = 0; i < Rules.GetRule(RuleType.BoardSize).MaxVal; i++) {
                ConversionDictionary.Add(i, ToBase26(i + 1));
            }
        }
        

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