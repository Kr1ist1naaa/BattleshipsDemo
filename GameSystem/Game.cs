using System;
using System.Collections.Generic;
using System.Text;
using BoardUI;
using Domain;
using Domain.Rule;
using Domain.Ship;

namespace GameSystem {
    public static class Game {
        // Todo: auto-place ships
        private static readonly Random Random = new Random();
        
        public static void LoadGame() {
            // Todo: this
        }
        
        public static void NewGame() {
            Console.Clear();
            Console.WriteLine("Welcome to Battleships - the classical battle ship game!\n");
            
            var players = InitializePlayers();

            var sb = new StringBuilder();
            sb.Append("\nPlayers ");
            foreach (var player in players) {
                sb.Append(player.Name);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(" were created...");
            Console.WriteLine(sb.ToString());
            Console.ReadKey(true);

            InitializeShips(players);
            GameLoop(players);
        }

        private static List<Player> InitializePlayers() {
            var playerCount = Rules.GetVal(RuleType.PlayerCount);
            var players = new List<Player>();
            
            Console.WriteLine($"Create {playerCount} players for the game:");
            
            for (var i = 0; i < playerCount; i++) {
                string name;
                
                Console.Write($"- name for player {i}: ");
                
                while (true) {
                    name = Console.ReadLine();
                    
                    // Check input validity
                    if (!string.IsNullOrEmpty(name) && name.Trim().Length != 0) {
                        break;
                    }

                    Console.Write("  - That's a no-go name. Enter it again: ");
                }
                
                players.Add(new Player(name));
            }

            return players;
        }
        
        private static void InitializeShips(IEnumerable<Player> players) {
            foreach (var player in players) {
                Console.Clear();
                Console.WriteLine($"Place {player.Name}'s ships...");
                Console.ReadKey(true);

                foreach (var ship in player.Ships) {
                    Console.Clear();
                    // Print the player's current board
                    BoardGen.GenSingleBoard(player, $"Place {player.Name}'s ships");
                    Console.WriteLine($"- {ship.Title} (size {ship.Size}):");
                
                    while (true) {
                        Console.Write("  - x: ");
                        int.TryParse(Console.ReadLine(), out var posX);

                        Console.Write("  - y: ");
                        int.TryParse(Console.ReadLine(), out var posY);
            
                        Console.Write("  - direction: ");
                        ShipDirection? direction = null;
                        switch (Console.ReadLine()?.ToLower().Trim()) {
                            case "right":
                            case "r":
                                direction = ShipDirection.Right;
                                break;
                            case "down":
                            case "d":
                                direction = ShipDirection.Down;
                                break;
                        }

                        if (direction == null) {
                            Console.WriteLine("Invalid direction! Use (right/r) or (down/d) and try again:");
                            continue;
                        }
                    
                        var pos = new Pos(posX, posY);
                        if (!player.CheckIfValidPlacementPos(pos, ship.Size, (ShipDirection) direction)) {
                            Console.WriteLine("Invalid position! Try again:");
                            continue;
                        }

                        ship.SetLocation(pos, (ShipDirection) direction);
                        break;
                    }
                }
            }
        }
        
        private static void GameLoop(IReadOnlyList<Player> players) {
            Player winner = null;
            var turnCount = 0;
            var moves = new List<Move>();
            
            Console.Clear();
            Console.WriteLine("Starting game...");
            Console.ReadKey(true);
            
            while (true) {
                foreach (var player in players) {
                    // Loop until first alive player is found
                    if (!player.IsAlive()) {
                        continue;
                    }
                    
                    Console.Clear();
                    Console.WriteLine($"It is now {player.Name}'s turn");
                    Console.ReadKey(true);
                    
                    // Find next player in list that is alive
                    var nextPlayer = FindNextPlayer(players, player);

                    Console.Clear();
                    
                    BoardGen.GenTwoBoards(player, nextPlayer);
                    Console.WriteLine($"Turn {turnCount}: {player.Name} is attacking {nextPlayer.Name}");
                                        
                    var move = Attack(player, nextPlayer);
                    moves.Add(move);
                    
                    Console.ReadKey(true);
                    
                    Console.Clear();
                    BoardGen.GenTwoBoards(player, nextPlayer);
                    Console.WriteLine($"Turn {turnCount}: {player.Name} is attacking {nextPlayer.Name}");
                    Console.WriteLine($"- attack at location {move.Pos.X}x {move.Pos.Y}y");
                    Console.WriteLine($"  - it was a {move.AttackResult.ToString()}");
                    Console.ReadKey(true);
                    
                    // Check if target player is out of the game (all ships have been destroyed)
                    if (!nextPlayer.IsAlive()) {
                        Console.Clear();
                        Console.WriteLine($"{player.Name} knocked {nextPlayer.Name} out of the game!");
                        Console.ReadKey(true);
                    }
                }


                // Check if there is only one player left and therefore the winner of the game
                foreach (var player in players) {
                    if (!player.IsAlive()) {
                        continue;
                    }
                    
                    if (winner == null) {
                        winner = player;
                    } else {
                        winner = null;
                        break;
                    }
                }
                
                if (winner != null) {
                    break;
                }
                
                Console.Clear();
                Console.WriteLine($"End of round {turnCount}");
                Console.ReadKey(true);

                turnCount++;
            }
            
            Console.Clear();
            Console.WriteLine($"The winner of the game is {winner.Name} after {turnCount} turns!");
            Console.ReadKey(true);
        }

        private static Player FindNextPlayer(IReadOnlyList<Player> players, Player player) {
            var playerCount = Rules.GetVal(RuleType.PlayerCount);
            Player nextPlayer = null;

            for (int i = 0; i < playerCount; i++) {
                // Find current player's index
                if (players[i] != player) {
                    continue;
                }
                
                for (int j = 1; j < playerCount; j++) {
                    var tmpPlayer = players[i + j - (i + j < playerCount ? 0 : playerCount)];

                    if (tmpPlayer.IsAlive()) {
                        nextPlayer = tmpPlayer;
                    }
                }
            }

            // Should not run
            if (nextPlayer == null) {
                throw new NullReferenceException(nameof(nextPlayer));
            }

            return nextPlayer;
        }
        
        private static Move Attack(Player player, Player targetPlayer) {
            AttackResult result;
            Pos pos;
            
            while (true) {
                Console.Write("  - x: ");
                int.TryParse(Console.ReadLine(), out var posX);

                Console.Write("  - y: ");
                int.TryParse(Console.ReadLine(), out var posY);
            
                pos = new Pos(posX, posY);
                result = targetPlayer.AttackAtPos(pos);

                if (result == AttackResult.InvalidAttack) {
                    Console.WriteLine("Invalid position! Try again:");
                    continue;
                } 
                
                if (result == AttackResult.DuplicateAttack) {
                    Console.WriteLine("Already attacked there! Try again:");
                    continue;
                }
                
                break;
            }
            
            return new Move(player, targetPlayer, pos, result);
        }
    }
}