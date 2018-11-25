using System;
using System.Collections.Generic;
using MenuSystem;

namespace Domain {
    public class Game {
        private readonly Menu _menu;
        
        private readonly List<Player> _players;
        private readonly List<Move> _moves;

        private readonly Pos _boardSize;
        private readonly int _playerCount;
        private readonly int _gameNumber;

        private int _turnCount;

        public Game(Menu menu, int gameNumber, int playerCount, Pos boardSize) {
            if (boardSize.X < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSize.X));
            }
            
            if (boardSize.Y < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSize.Y));
            }

            if (playerCount < 2) {
                throw new ArgumentOutOfRangeException(nameof(playerCount));
            }
            
            if (gameNumber < 0) {
                throw new ArgumentOutOfRangeException(nameof(gameNumber));
            }

            _menu = menu;
            _boardSize = new Pos(boardSize);
            _playerCount = playerCount;
            _gameNumber = gameNumber;

            _players = new List<Player>();
            _moves = new List<Move>();

            InitializePlayers();
        }

        private void InitializePlayers() {
            Player lastPlayer = null, firstPlayer = null;
            
            Console.WriteLine($"- creating {_playerCount} players for game nr {_gameNumber}");
            
            for (var i = 0; i < _playerCount; i++) {
                var firstLoop = true;
            
                while (true) {
                    // Ask player for name
                    firstLoop = _menu.AskPlayerName(firstLoop, i, out var name);
                    
                    // Check if name is valid
                    var isValidName = !string.IsNullOrEmpty(name);
                    if (!isValidName) continue;
                    
                    Console.WriteLine($"  - creating player {name} as player nr {i + 1}");
                    var player = new Player(_menu, _boardSize, name, i + 1);
                    
                    Console.WriteLine($"    - player {name} is now placing their ships");
                    player.PlaceShips();
                    
                    _players.Add(player);
                    
                    if (firstPlayer == null) {
                        firstPlayer = player;
                    } else {
                        lastPlayer.TargetPlayer = player;
                    }

                    lastPlayer = player;
                    
                    break;
                }
            }

            if (lastPlayer != null) {
                lastPlayer.TargetPlayer = firstPlayer;
            }

            Console.WriteLine($"- finished creating {_playerCount} players for game nr {_gameNumber}");
        }
        
        public void StartGame() {
            Player winner = null;
            
            Console.WriteLine("- starting game");
            
            while (true) {
                Console.WriteLine($"  - beginning of turn {_turnCount}");

                for (int i = 0; i < _playerCount; i++) {
                    var player = _players[i];
                    
                    Console.WriteLine($"    - player {player.Name}'s turn to attack {player.TargetPlayer.Name}");
                    var move = player.AttackTargetPlayer();
                    _moves.Add(move);
                    
                    // Check if target player is out of the game (all ships have been hit)
                    if (!player.TargetPlayer.CheckIsAlive()) {
                        Console.WriteLine($"    - {player.Name} knocked {player.TargetPlayer.Name} out of the game!");

                        // Reset target player to target player's target player
                        var tmpPlayer = player.TargetPlayer;
                        player.TargetPlayer = player.TargetPlayer.TargetPlayer;
                        tmpPlayer.TargetPlayer = null;
                    }
                    
                    // Check if current player is the only player left and therefore the winner of the game
                    if (player.TargetPlayer.Equals(player)) {
                        return;
                    }
                }
                
                Console.WriteLine($"    - end of turn {++_turnCount}");

                if (_turnCount > 100) {
                    break;
                }
                
                // Check if there's only one player left who is therefore the winner of the game
                foreach (var player in _players) {
                    if (winner != null && player.CheckIsAlive()) {
                        winner = null;
                        break;
                    }
                    
                    if (player.CheckIsAlive()) {
                        winner = player;
                    }
                }

                if (winner != null) {
                    break;
                }
            }
            
            Console.WriteLine($"- the winner of game {_gameNumber} is {winner?.Name} after {_turnCount} turns!");
        }
    }
}