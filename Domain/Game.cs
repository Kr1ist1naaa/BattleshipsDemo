using System;
using System.Collections.Generic;

namespace Domain {
    public class Game {
        private readonly Menu _menu;
        private readonly List<Player> _players;
        private readonly List<Rule> _rules;
        private readonly List<Move> _moves;
        private readonly int _gameNumber;
        private int _turnCount;

        public Game(Menu menu, int gameNumber) {
            _menu = menu;
            _gameNumber = gameNumber;
            _rules = Rule.GenBaseRuleSet();
            
            InitializeRules();
            
            _players = new List<Player>();
            _moves = new List<Move>();

            InitializePlayers();
        }

        private void InitializeRules() {
            Console.WriteLine($"- creating rules for game nr {_gameNumber}");

            foreach (var rule in Rule.GenBaseRuleSet()) {
                var firstLoop = true;
                
                while (true) {
                    // If rule should be set elsewhere
                    if (!rule.Ask) {
                        break;
                    }
                    
                    // Ask player for rule value
                    firstLoop = _menu.AskBaseRule(firstLoop, rule);

                    if (rule.RuleName == Rule.BoardSize.RuleName) {
                        if (rule.Value < 2 || rule.Value > 128) {
                            Console.WriteLine($"  - invalid value {rule.Value} for rule {rule.RuleName}");
                            continue;
                        }
                    } else if (rule.RuleName == Rule.PlayerCount.RuleName) {
                        if (rule.Value < 2 || rule.Value > 128) {
                            Console.WriteLine($"  - invalid value {rule.Value} for rule {rule.RuleName}");
                             continue;
                        }
                    } else if (rule.RuleName == Rule.ShipCount.RuleName) {
                        if (rule.Value < 1 || rule.Value > 128) {
                            Console.WriteLine($"  - invalid value {rule.Value} for rule {rule.RuleName}");
                            continue;
                        }
                    } else if (rule.RuleName == Rule.ShipsCanTouch.RuleName) {
                        if (rule.Value < 0 || rule.Value > 1) {
                            Console.WriteLine($"  - invalid value {rule.Value} for rule {rule.RuleName}");
                            continue;
                        }
                    }
                    
                    break;
                }
            }
            
            foreach (var rule in _rules) {
                Console.WriteLine($"  - created rule {rule.RuleName} with value {rule.Value}");
            }
        }

        private void InitializePlayers() {
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);
            Console.WriteLine($"- creating {playerCount} players for game nr {_gameNumber}");
            
            for (var i = 0; i < playerCount; i++) {
                var firstLoop = true;
            
                while (true) {
                    // Ask player for name
                    firstLoop = _menu.AskPlayerName(firstLoop, i, out var name);
                    
                    // Check if name is valid
                    var isValidName = !string.IsNullOrEmpty(name);
                    if (!isValidName) continue;
                    
                    Console.WriteLine($"  - creating player {name} as player nr {i}");
                    var player = new Player(_menu, _rules, name, i);
                    
                    Console.WriteLine($"    - player {name} is now placing their ships");
                    player.PlaceShips();

                    _players.Add(player);
                    break;
                }
            }

            Console.WriteLine($"- finished creating {playerCount} players for game nr {_gameNumber}");
        }
        
        public void StartGame() {
            var playerCount = Rule.GetRule(_rules, Rule.PlayerCount);
            Player winner = null;
            Console.WriteLine("- starting game");
            
            while (true) {
                Console.WriteLine($"  - beginning of turn {_turnCount}");

                for (int i = 0; i < playerCount; i++) {
                    var player = _players[i];
                    
                    // Loop until alive player is found
                    if (!player.IsAlive()) {
                        continue;
                    }
                    
                    // Find next player in list that is alive
                    var nextPlayer = FindNextPlayer(player);

                    Console.WriteLine($"    - player {player.Name}'s turn to attack {nextPlayer.Name}");
                    
                    var move = player.AttackPlayer(nextPlayer);
                    
                    _moves.Add(move);

                    player.GenBoard();
                    
                    // Check if target player is out of the game (all ships have been destroyed)
                    if (!nextPlayer.IsAlive()) {
                        Console.WriteLine($"    - {player.Name} knocked {nextPlayer.Name} out of the game!");
                    }
                }

                Console.WriteLine($"    - end of turn {++_turnCount}");

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
            }
            
            Console.WriteLine($"- the winner of game {_gameNumber} is {winner.Name} after {_turnCount} turns!");
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