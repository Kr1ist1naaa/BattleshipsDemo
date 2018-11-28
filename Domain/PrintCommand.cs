using System;
using System.Collections.Generic;

namespace Domain {
    public class PrintCommand {
        private readonly string _str;
        private readonly ConsoleColor? _color;

        public PrintCommand(string str, ConsoleColor? color = null) {
            _str = str;
            _color = color;
        }

        public static void Print(IEnumerable<PrintCommand> commands) {
            foreach (var command in commands) {
                if (command._color == null) {
                    Console.Write(command._str);
                } else {
                    Console.ForegroundColor = command._color.Value;
                    Console.Write(command._str);
                    Console.ResetColor();
                }
            }
        }
    }
}