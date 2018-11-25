using System;
using System.Collections.Generic;
using Domain.Ship;
using MenuSystem;

namespace Domain {
    public class Player {
        private readonly Menu _menu;
        public readonly string Name;
        public readonly int Number;
        private readonly Pos _boardSize;
        private readonly HashSet<Pos> _movesAgainstThisPlayer;
        private readonly List<Ship.Ship> _ships;

        public Player TargetPlayer = null; // Reference to the player this player will attack

        public Player(Menu menu, Pos size, string playerName, int playerNumber) {
            if (size.X < 2) {
                throw new ArgumentOutOfRangeException(nameof(size.X));
            }

            if (size.Y < 2) {
                throw new ArgumentOutOfRangeException(nameof(size.Y));
            }

            if (string.IsNullOrEmpty(playerName)) {
                throw new ArgumentOutOfRangeException(nameof(playerName));
            }

            if (playerNumber < 1) {
                throw new ArgumentOutOfRangeException(nameof(playerNumber));
            }

            _ships = Ship.Ship.GenDefaultShipSet();
            _movesAgainstThisPlayer = new HashSet<Pos>();
            _menu = menu;

            Name = playerName;
            Number = playerNumber;

            _boardSize = new Pos(size);
        }

        public Move AttackTargetPlayer() {
            if (TargetPlayer == null) {
                throw new NullReferenceException(nameof(TargetPlayer));
            }

            var firstLoop = true;
            Pos pos;

            while (true) {
                // Ask player for attack location
                firstLoop = _menu.AskAttackCoords(firstLoop, out var posX, out var posY);
                pos = new Pos(posX, posY);

                // Check if attack coords are valid
                var validAttackPos = CheckValidAttackPos(TargetPlayer, pos);
                if (!validAttackPos) continue;

                Console.WriteLine($"      - attack at location {pos.X}x {pos.Y}y");

                var validEnemyShip = GetValidEnemyShipOrNull(TargetPlayer, pos);
                validEnemyShip?.AttackShipAtPos(pos);

                TargetPlayer._movesAgainstThisPlayer.Add(pos);
                break;
            }

            // Create instance of Move with the details of the move
            return new Move(this, TargetPlayer, pos);
        }

        public void PlaceShips() {
            foreach (var ship in _ships) {
                var firstLoop = true;

                while (true) {
                    // Ask player for placement location
                    firstLoop = _menu.AskShipPlacementPosition(firstLoop, out var posX, out var posY, out var dir);
                    var pos = new Pos(posX, posY);

                    ShipDirection direction;
                    switch (dir) {
                        case "right":
                            direction = ShipDirection.Right;
                            break;
                        case "down":
                            direction = ShipDirection.Down;
                            break;
                        default:
                            continue;
                    }

                    // Check if player has already attacked there
                    var isValidPos = CheckValidShipPlacementSpot(pos, ship.Size, direction);
                    if (!isValidPos) continue;

                    // Place the ship
                    ship.SetLocation(pos, direction);

                    Console.WriteLine($"        - placed a {ship.Title} at coords {pos.X}x {pos.Y}y, direction {dir}");
                    break;
                }
            }
        }

        private static bool CheckValidAttackPos(Player player, Pos pos) {
            // Out of bounds
            if (pos.X < 0 || pos.X >= player._boardSize.X) return false;
            if (pos.Y < 0 || pos.Y >= player._boardSize.Y) return false;

            // Player has already been attacked there
            return !player._movesAgainstThisPlayer.Contains(pos);
        }

        private Ship.Ship GetValidEnemyShipOrNull(Player targetPlayer, Pos pos) {
            if (pos.X < 0 || pos.X >= _boardSize.X) return null;
            if (pos.Y < 0 || pos.Y >= _boardSize.Y) return null;

            foreach (var ship in targetPlayer._ships) {
                if (!ship.IsShipAtPos(pos)) {
                    continue;
                }

                if (ship.CanAttackShipAtPos(pos)) {
                    return ship;
                }
            }

            return null;
        }

        private bool CheckValidShipPlacementSpot(Pos pos, int shipSize, ShipDirection direction) {
            // Check if ship would be off the board
            var maxPosX = pos.X + (direction == ShipDirection.Right ? shipSize : 0);
            var maxPosY = pos.Y + (direction == ShipDirection.Right ? 0 : shipSize);

            if (pos.X < 0 || maxPosX >= _boardSize.X) return false;
            if (pos.Y < 0 || maxPosY >= _boardSize.Y) return false;

            // Check if any ships already exist at that location
            foreach (var ship in _ships) {
                if (ship.IsIntersect(pos, shipSize, direction)) {
                    return false;
                }
            }

            return true;
        }

        public bool CheckIsAlive() {
            foreach (var ship in _ships) {
                if (!ship.IsDestroyed()) {
                    return true;
                }
            }

            return false;
        }
    }
}