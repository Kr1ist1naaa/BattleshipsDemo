using System;
using System.Collections.Generic;
using System.Text;
using Domain.Ship;

namespace Domain {
    public class Player {
        private readonly Menu _menu;
        public readonly string Name;
        public readonly int PlayerNumber;

        private readonly HashSet<Pos> _movesAgainstThisPlayer;
        private readonly List<Rule> _rules;
        private List<Ship.Ship> _ships;

        public Player(Menu menu, List<Rule> rules, string playerName, int playerNumber) {
            if (string.IsNullOrEmpty(playerName)) {
                throw new ArgumentOutOfRangeException(nameof(playerName));
            }

            if (playerNumber < 0) {
                throw new ArgumentOutOfRangeException(nameof(playerNumber));
            }

            _movesAgainstThisPlayer = new HashSet<Pos>();
            _menu = menu;
            Name = playerName;
            PlayerNumber = playerNumber;
            _rules = rules;
        }

        public Move AttackPlayer(Player target) {
            if (target == null) {
                throw new NullReferenceException(nameof(target));
            }

            AttackResult attackResult;
            Pos pos;

            while (true) {
                // Ask player for attack location
                _menu.AskAttackCoords(out var posX, out var posY);
                pos = new Pos(posX, posY);

                // If method returned non-null ship, attack it
                attackResult = target.AttackAtPos(pos);

                if (attackResult == AttackResult.InvalidAttack) {
                    Console.WriteLine($"- invalid attack at {pos.X}x {pos.Y}y");
                    continue;
                } 
                
                if (attackResult == AttackResult.DuplicateAttack) {
                    Console.WriteLine($"- can't attack duplicate location {pos.X}x {pos.Y}y");
                    continue;
                }
                
                break;
            }

            return new Move(this, target, pos, attackResult);
        }

        public void PlaceShips() {
            // Generate a set of ships for the player based on the current rules
            _ships = Ship.Ship.GenShipSet(_rules);
            
            foreach (var ship in _ships) {
                while (true) {
                    Console.Clear();
                    PrintBoard(false);
                    Console.WriteLine($"Please place {Name}'s ships: ");
                    Console.WriteLine($"  - place {ship.Title} (size {ship.Size}):");

                    // Ask player for placement location
                    _menu.AskShipPlacementPosition(out var posX, out var posY, out var dir);
                    var pos = new Pos(posX, posY);

                    ShipDirection direction;
                    switch (dir.ToLower()) {
                        case "right":
                        case "r":
                            direction = ShipDirection.Right;
                            break;
                        case "down":
                        case "d":
                            direction = ShipDirection.Down;
                            break;
                        default:
                            Console.WriteLine("  - invalid direction");
                            continue;
                    }

                    // Check if player has already attacked there
                    var isValidPos = CheckIfValidPlacementPos(pos, ship.Size, direction);
                    if (!isValidPos) {
                        Console.WriteLine("  - invalid position!:");
                        continue;
                    }
                    
                    Console.ReadKey(true);

                    // Place the ship
                    ship.SetLocation(pos, direction);
                    break;
                }
            }
        }

        private bool CheckIfPosInBoard(Pos pos) {
            var boardSize = Rule.GetRule(_rules, Rule.BoardSize);
            
            // Out of bounds
            if (pos.X < 0 || pos.X >= boardSize) return false;
            if (pos.Y < 0 || pos.Y >= boardSize) return false;

            return true;
        }

        private AttackResult AttackAtPos(Pos pos) {
            if (!CheckIfPosInBoard(pos)) {
                return AttackResult.InvalidAttack;
            }

            if (_movesAgainstThisPlayer.Contains(pos)) {
                return AttackResult.DuplicateAttack;
            }

            _movesAgainstThisPlayer.Add(pos);
            
            var ship = GetShipAtPosOrNull(pos);
            if (ship == null) {
                return AttackResult.Miss;
            }

            return ship.AttackAtPos(pos);
        }
        
        private Ship.Ship GetShipAtPosOrNull(Pos pos) {
            foreach (var ship in _ships) {
                if (ship.IsAtPos(pos)) {
                    return ship;
                }
            }

            return null;
        }

        private bool CheckIfValidPlacementPos(Pos pos, int shipSize, ShipDirection direction) {
            // Check if position is off board
            if (!CheckIfPosInBoard(pos)) {
                return false;
            }
            
            // Find ship's furthest point
            var maxPos = new Pos(
                pos.X + (direction == ShipDirection.Right ? shipSize : 0),
                pos.Y + (direction == ShipDirection.Right ? 0 : shipSize)
            );

            // Check if max position is off board
            if (!CheckIfPosInBoard(maxPos)) {
                return false;
            }
            
            var padding = Rule.GetRule(_rules, Rule.ShipPadding);

            // Check if any ships already exist at that location
            foreach (var ship in _ships) {
                if (ship.CheckIfIntersect(pos, shipSize, direction, padding)) {
                    return false;
                }
            }

            return true;
        }

        public bool IsAlive() {
            // Count each ship block individually, checking if there is at least one that's not hit
            foreach (var ship in _ships) {
                if (!ship.IsDestroyed()) {
                    return true;
                }
            }

            return false;
        }

        public void PrintBoard(bool isPrivate) {
            var boardSize = Rule.GetRule(_rules, Rule.BoardSize);

            // Generate horizontal border
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < boardSize; i++) 
                stringBuilder.Append("+-----");
            stringBuilder.Append("+");
            var border = stringBuilder.ToString();
            
            for (int i = 0; i < boardSize; i++) {
                Console.WriteLine(border);

                for (int j = 0; j < boardSize; j++) {
                    Console.Write("|  ");

                    var pos = new Pos(i, j);
                    var ship = GetShipAtPosOrNull(pos);

                    if (ship == null) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(_movesAgainstThisPlayer.Contains(pos) ? "." : " ");
                        Console.ResetColor();
                    } else if (ship.IsDestroyed()) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(ship.Symbol.ToString());
                        Console.ResetColor();
                    } else {
                        if (_movesAgainstThisPlayer.Contains(pos)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(ship.Symbol.ToString());
                            Console.ResetColor();
                        } else {
                            Console.Write(isPrivate ? " " : ship.Symbol.ToString());
                        }
                    }
                    
                    Console.Write("  ");
                }
                
                Console.WriteLine("|");
            }
            
            Console.WriteLine(border);
        }
    }
}