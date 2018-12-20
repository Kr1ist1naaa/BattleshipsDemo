using System;
using System.Text;
using Domain;

namespace BoardUI {
    public static class BoardGen {
        public static Func<int, string> MapToBase26 { private get; set; }
        public static Func<Player, Pos, Ship> GetShipOrNull { private get; set; }
        public static Func<Ship, bool> IsShipDestroyed { private get; set; }
        public static Func<RuleType?, int> GetRuleVal { private get; set; }
        
        
        public static void GenSingleBoard(Player player, string title) {
            var boardSize = GetRuleVal(RuleType.BoardSize);
            var border = GenHorizontalBoardEdge();

            // Center and print title
            Console.WriteLine(GenCenterTitle(title));
            Console.WriteLine(GenColumnNumberings());

            for (int y = 0; y < boardSize; y++) {
                Console.WriteLine(border);

                // Row numbering
                Console.Write($"{y + 1,4} ");

                for (int x = 0; x < boardSize; x++) {
                    Console.Write("|  ");

                    var pos = new Pos(x, y);
                    var ship = GetShipOrNull(player, pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(player.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (IsShipDestroyed(ship)) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(ship.Symbol.ToString());
                        Console.ResetColor();
                    } else {
                        if (player.MovesAgainstThisPlayer.Contains(pos)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        } else {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        }
                    }

                    Console.Write("  ");
                }

                Console.WriteLine("|");
            }

            Console.WriteLine(border);
        }

        public static void GenTwoBoards(Player player, Player nextPlayer) {
            var boardSize = GetRuleVal(RuleType.BoardSize);
            const string gap = "     ";
            var border = GenHorizontalBoardEdge();

            // Center and print "Your board:" and "Enemy's board:"
            var title = GenCenterTitle("Your board") + gap + GenCenterTitle("Enemy's board");
            Console.WriteLine(title);

            var colNumbs = GenColumnNumberings();
            Console.WriteLine(colNumbs + gap + colNumbs);

            for (int y = 0; y < boardSize; y++) {
                Console.WriteLine(border + gap + border);

                // Row numbering
                Console.Write($"{y+1,4} ");

                // Your board horizontal line
                for (int x = 0; x < boardSize; x++) {
                    Console.Write("|  ");

                    var pos = new Pos(x, y);
                    var ship = GetShipOrNull(player, pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(player.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (IsShipDestroyed(ship)) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(ship.Symbol.ToString());
                        Console.ResetColor();
                    } else {
                        if (player.MovesAgainstThisPlayer.Contains(pos)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        } else {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        }
                    }

                    Console.Write("  ");
                }

                // Row numbering
                Console.Write($"|{gap}{y+1,4} ");

                // Enemy board horizontal line
                for (int x = 0; x < boardSize; x++) {
                    Console.Write("|  ");

                    var pos = new Pos(x, y);
                    var ship = GetShipOrNull(nextPlayer, pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(nextPlayer.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (IsShipDestroyed(ship)) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(ship.Symbol.ToString());
                        Console.ResetColor();
                    } else {
                        if (nextPlayer.MovesAgainstThisPlayer.Contains(pos)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        } else {
                            Console.Write(" ");
                        }
                    }

                    Console.Write("  ");
                }

                Console.WriteLine("|");
            }

            Console.WriteLine(border + gap + border);
        }

        private static string GenColumnNumberings() {
            var boardSize = GetRuleVal(RuleType.BoardSize);
            var sb = new StringBuilder();

            sb.Append("     ");
            for (int i = 0; i < boardSize; i++) {
                sb.Append($"{MapToBase26(i),4}  ");
            }

            return sb.ToString();
        }

        private static string GenHorizontalBoardEdge() {
            var boardSize = GetRuleVal(RuleType.BoardSize);
            var sb = new StringBuilder();

            sb.Append("     ");
            for (int i = 0; i < boardSize; i++) {
                sb.Append("+-----");
            }

            sb.Append("+");

            return sb.ToString();
        }

        private static string GenCenterTitle(string title) {
            var borderLen = GenHorizontalBoardEdge().Length;
            var sb = new StringBuilder();

            var leftPad = (borderLen - title.Length) / 2 + 5;
            var rightPad = borderLen - title.Length - leftPad;

            sb.Append(new string(' ', leftPad));
            sb.Append(title);
            sb.Append(new string(' ', rightPad));

            return sb.ToString();
        }
    }
}