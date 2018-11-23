using System;
using System.Collections.Generic;
using MenuSystem;

namespace Domain {
    public class Game {
        private readonly Menu _menu;
        
        private readonly List<Player> _players;
        private readonly List<Move> _moves;

        private readonly int _boardSizeX;
        private readonly int _boardSizeY;
        private readonly int _playerCount;
        private readonly int _gameNumber;

        private int _turnCount;

        public Game(Menu menu, int gameNumber, int playerCount, int boardSizeX, int boardSizeY) {
            if (boardSizeX < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeX));
            }
            
            if (boardSizeY < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeY));
            }

            if (playerCount < 2) {
                throw new ArgumentOutOfRangeException(nameof(playerCount));
            }
            
            if (gameNumber < 0) {
                throw new ArgumentOutOfRangeException(nameof(gameNumber));
            }

            _menu = menu;
            _boardSizeX = boardSizeX;
            _boardSizeY = boardSizeY;
            _playerCount = playerCount;
            _gameNumber = gameNumber;

            _players = new List<Player>();
            _moves = new List<Move>();

            InitializePlayers();
        }

        private void InitializePlayers() {
            Player lastPlayer = null, firstPlayer = null;
            
            Console.WriteLine($"Creating {_playerCount} players for game nr {_gameNumber}");
            
            for (var i = 0; i < _playerCount; i++) {
                var firstLoop = true;
            
                while (true) {
                    // Ask player for name
                    firstLoop = _menu.AskPlayerName(firstLoop, i, out var name);
                    
                    // Check if name is valid
                    var isValidName = !string.IsNullOrEmpty(name);
                    if (!isValidName) continue;
                    
                    Console.WriteLine($"  - creating player {name} as player nr {i + 1}");

                    // Create player
                    var player = new Player(_menu, _boardSizeX, _boardSizeY, name);
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

            Console.WriteLine($"Finished creating {_playerCount} players for game nr {_gameNumber}");
        }
        
        public void StartGame() {
            Player winner = null;
            
            while (true) {
                Console.WriteLine($"Beginning of turn {_turnCount}");

                for (int i = 0; i < _playerCount; i++) {
                    var player = _players[i];
                    
                    Console.WriteLine($"Player {player.Name}'s turn to attack {player.TargetPlayer.Name}");
                    var move = player.OutgoingAttack();
                    _moves.Add(move);
                    
                    // Check if target player is out of the game (all ships have been hit)
                    if (!player.TargetPlayer.IsAlive) {
                        Console.WriteLine($"{player.Name} knocked {player.TargetPlayer.Name} out of the game!");

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
                
                Console.WriteLine($"End of turn {++_turnCount}\n");
                
                // Check if there's only one player left who is therefore the winner of the game
                foreach (var player in _players) {
                    if (winner != null && player.IsAlive) {
                        winner = null;
                        break;
                    }
                    
                    if (player.IsAlive) {
                        winner = player;
                    }
                }

                if (winner != null) {
                    break;
                }
            }
            
            Console.WriteLine($"The winner of game {_gameNumber} is {winner.Name} after {_turnCount} turns!");
        }
    }
}