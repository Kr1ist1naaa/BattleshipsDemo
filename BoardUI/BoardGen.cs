using System;
using System.Text;
using Domain;
using Domain.DomainRule;

namespace BoardUI {
    public static class BoardGen {
        public static void GenSingleBoard(Player player, string title) {
            var boardSize = Rules.GetVal(RuleType.BoardSize);
            var border = GenHorizontalBoardEdge();

            // Center and print title
            Console.WriteLine(GenCenterTitle(title));

            for (int i = 0; i < boardSize; i++) {
                Console.WriteLine(border);

                for (int j = 0; j < boardSize; j++) {
                    Console.Write("|  ");

                    var pos = new Pos(j, i);
                    var ship = player.GetShipAtPosOrNull(pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(player.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (ship.IsDestroyed()) {
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
            var boardSize = Rules.GetVal(RuleType.BoardSize);
            const string gap = "     ";
            var border = GenHorizontalBoardEdge();

            // Center and print "Your board:" and "Enemy's board:"
            var title = GenCenterTitle("Your board") + gap + GenCenterTitle("Enemy's board");
            Console.WriteLine(title);

            for (int i = 0; i < boardSize; i++) {
                Console.WriteLine(border + gap + border);

                // Your board horizontal line
                for (int j = 0; j < boardSize; j++) {
                    Console.Write("|  ");

                    var pos = new Pos(i, j);
                    var ship = player.GetShipAtPosOrNull(pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(player.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (ship.IsDestroyed()) {
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

                Console.Write("|" + gap);

                // Enemy board horizontal line
                for (int j = 0; j < boardSize; j++) {
                    Console.Write("|  ");

                    var pos = new Pos(i, j);
                    var ship = nextPlayer.GetShipAtPosOrNull(pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(nextPlayer.MovesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (ship.IsDestroyed()) {
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

        private static string GenHorizontalBoardEdge() {
            var boardSize = Rules.GetVal(RuleType.BoardSize);
            var sb = new StringBuilder();

            for (int i = 0; i < boardSize; i++) {
                sb.Append("+-----");
            }
            sb.Append("+");
            
            return sb.ToString();
        }

        private static string GenCenterTitle(string title) {
            var borderLen = GenHorizontalBoardEdge().Length;
            var sb = new StringBuilder();

            var leftPad = (borderLen - title.Length) / 2;
            var rightPad = borderLen - title.Length - leftPad;

            sb.Append(new string(' ', leftPad));
            sb.Append(title);
            sb.Append(new string(' ', rightPad));

            return sb.ToString();
        }
    }
}