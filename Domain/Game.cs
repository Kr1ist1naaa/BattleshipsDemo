using System;
using System.Collections.Generic;

namespace Domain {
    public class Game {
        private static int _gameNumber = -1;
        
        private readonly Menu _menu;
        private readonly List<Player> _players;
        private readonly List<Rule> _rules;
        private readonly List<Move> _moves;
        private int _turnCount;

        public Game(Menu menu) {
            _gameNumber++;
            
            Console.WriteLine($"  - starting game nr {_gameNumber}...");
            Console.ReadKey(true);
            
            _menu = menu;
            _rules = Rule.GenBaseRuleSet();
            _players = new List<Player>();
            _moves = new List<Move>();
        }
 
        public void InitializeRules() {
            Console.Clear();
            Console.WriteLine($"Please define rules for game nr {_gameNumber}:");

            foreach (var rule in _rules) {
                while (true) {
                    // If rule should be set elsewhere
                    if (!rule.AskOnInit) {
                        break;
                    }
                    
                    // Ask player for rule value
                    _menu.AskBaseRule(rule);

                    if (rule.RuleName == Rule.BoardSize.RuleName) {
                        if (rule.Value < 2 || rule.Value > 128) {
                            Console.WriteLine("   - invalid value");
                            continue;
                        }
                    } else if (rule.RuleName == Rule.PlayerCount.RuleName) {
                        if (rule.Value < 2 || rule.Value > 128) {
                            Console.WriteLine("   - invalid value");
                             continue;
                        }
                    } else if (rule.RuleName == Rule.ShipPadding.RuleName) {
                        if (rule.Value < 0 || rule.Value > 1) {
                            Console.WriteLine("   - invalid value");
                            continue;
                        }
                    }
                    
                    Console.ReadKey(true);
                    
                    break;
                }
            }
        }

        public void InitializePlayers() {
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);
            
            Console.Clear();
            Console.WriteLine("Please create players:");
            
            for (var i = 0; i < playerCount; i++) {
                while (true) {
                    Console.WriteLine($"  - player {i}  : ");
                    
                    // Ask player for name
                    _menu.AskPlayerName(i, out var name);
                    
                    // Check if name is valid
                    var isValidName = !string.IsNullOrEmpty(name);
                    if (!isValidName) continue;

                    var player = new Player(_menu, _rules, name, i);

                    _players.Add(player);
                    break;
                }
                
                Console.ReadKey(true);
            }
        }

        public void InitializeShips() {
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);

            for (var i = 0; i < playerCount; i++) {
                var player = _players[i];
                
                Console.Clear();
                Console.WriteLine($"Please place {player.Name}'s ships...");
                Console.ReadKey(true);

                player.PlaceShips();
            }
        }
        
        public void StartGame() {
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);
            Player winner = null;
            
            Console.Clear();
            Console.WriteLine("Starting game...");
            Console.ReadKey(true);
            
            while (true) {
                for (int i = 0; i < playerCount; i++) {
                    var player = _players[i];
                    
                    // Loop until alive player is found
                    if (!player.IsAlive()) {
                        continue;
                    }
                    
                    Console.Clear();
                    Console.WriteLine($"It is now {player.Name}'s turn");
                    Console.ReadKey(true);
                    
                    // Find next player in list that is alive
                    var nextPlayer = FindNextPlayer(player);

                    Console.Clear();
                    player.PrintTwoBoards(nextPlayer);
                    Console.WriteLine($"Turn {_turnCount}: {player.Name} is attacking {nextPlayer.Name}");
                                        
                    var move = player.AttackPlayer(nextPlayer);
                    _moves.Add(move);
                    
                    Console.ReadKey(true);
                    
                    Console.Clear();
                    player.PrintTwoBoards(nextPlayer);
                    Console.WriteLine($"Turn {_turnCount}: {player.Name} is attacking {nextPlayer.Name}");
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
                for (int i = 0; i < playerCount; i++) {
                    if (!_players[i].IsAlive()) {
                        continue;
                    }
                    
                    if (winner == null) {
                        winner = _players[i];
                    } else {
                        winner = null;
                        break;
                    }
                }
                
                if (winner != null) {
                    break;
                }
                
                Console.Clear();
                Console.WriteLine($"End of round {_turnCount}");
                Console.ReadKey(true);

                _turnCount++;
            }
            
            Console.Clear();
            Console.WriteLine($"The winner of game {_gameNumber} is {winner.Name} after {_turnCount} turns!");
            Console.ReadKey(true);
        }

        private Player FindNextPlayer(Player player) {
            if (player == null) {
                throw new NullReferenceException(nameof(player));
            }
            
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);
            Player nextPlayer = null;

            for (int i = 0; i < playerCount; i++) {
                // Find current player's index
                if (_players[i] != player) {
                    continue;
                }
                
                for (int j = 1; j < playerCount; j++) {
                    var tmpPlayer = _players[i + j - (i + j < playerCount ? 0 : playerCount)];

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
    }
}