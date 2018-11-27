using System;
using System.Collections.Generic;
using MenuSystem;

namespace Domain {
    public class Game {
        private readonly Menu _menu;
        private readonly Player[] _players;
        private readonly List<Move> _moves;
        private readonly Pos _boardSize;
        private readonly int _playerCount;
        private readonly int _gameNumber;
        private int _turnCount;

        private readonly bool _ruleBoatsCanTouch = false;

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

            _players = new Player[_playerCount];
            _moves = new List<Move>();

            InitializePlayers();
        }

        private void InitializePlayers() {
            Console.WriteLine($"- creating {_playerCount} players for game nr {_gameNumber}");
            
            for (var i = 0; i < _playerCount; i++) {
                var firstLoop = true;
            
                while (true) {
                    // Ask player for name
                    firstLoop = _menu.AskPlayerName(firstLoop, i, out var name);
                    
                    // Check if name is valid
                    var isValidName = !string.IsNullOrEmpty(name);
                    if (!isValidName) continue;
                    
                    Console.WriteLine($"  - creating player {name} as player nr {i}");
                    var player = new Player(_menu, _boardSize, name, i);
                    
                    Console.WriteLine($"    - player {name} is now placing their ships");
                    player.PlaceShips(_ruleBoatsCanTouch);

                    _players[i] = player;
                    break;
                }
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
                for (int i = 0; i < _playerCount; i++) {
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
            
            Player nextPlayer = null;

            for (int i = 0; i < _playerCount; i++) {
                // Find current player's index
                if (_players[i] != player) {
                    continue;
                }
                
                for (int j = 1; j < _playerCount; j++) {
                    var tmpPlayer = _players[i + j - (i + j < _playerCount ? 0 : _playerCount)];

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