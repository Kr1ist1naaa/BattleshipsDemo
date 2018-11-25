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
        private bool _isAlive = true;

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

            if (playerNumber < 0) {
                throw new ArgumentOutOfRangeException(nameof(playerNumber));
            }

            _ships = Ship.Ship.GenDefaultShipSet();
            _movesAgainstThisPlayer = new HashSet<Pos>();
            _menu = menu;

            Name = playerName;
            Number = playerNumber;

            _boardSize = new Pos(size);
        }

        public Move AttackPlayer(Player target) {
            if (target == null) {
                throw new NullReferenceException(nameof(target));
            }

            var firstLoop = true;
            Pos pos;

            while (true) {
                // Ask player for attack location
                firstLoop = _menu.AskAttackCoords(firstLoop, out var posX, out var posY);
                pos = new Pos(posX, posY);

                // Check if attack coords are inside game board dimensions
                var validAttackPos = CheckIfPosInBoard(target, pos);
                if (!validAttackPos) {
                    Console.WriteLine($"      - can't attack invalid location {pos.X}x {pos.Y}y");
                    continue;
                }

                // Check if player has been attacked there before
                validAttackPos = CheckIfDuplicateAttack(target, pos);
                if (!validAttackPos) {
                    Console.WriteLine($"      - can't attack duplicate location {pos.X}x {pos.Y}y");
                    continue;
                }

                Console.WriteLine($"      - attack at location {pos.X}x {pos.Y}y");

                // If method returned non-null ship, attack it
                GetValidShipOrNull(target, pos)?.AttackShipAtPos(pos);

                target._movesAgainstThisPlayer.Add(pos);
                break;
            }

            // Create instance of Move with the details of the move
            return new Move(this, target, pos);
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
                    var isValidPos = CheckIfValidPlacementPos(pos, ship.Size, direction);
                    if (!isValidPos) continue;

                    // Place the ship
                    ship.SetLocation(pos, direction);

                    Console.WriteLine($"        - placed a {ship.Title} at coords {pos.X}x {pos.Y}y, direction {dir}");
                    break;
                }
            }
        }

        private static bool CheckIfPosInBoard(Player player, Pos pos) {
            // Out of bounds
            if (pos.X < 0 || pos.X >= player._boardSize.X) return false;
            if (pos.Y < 0 || pos.Y >= player._boardSize.Y) return false;

            return true;
        }

        private static bool CheckIfDuplicateAttack(Player player, Pos pos) {
            // Player has already been attacked there
            return !player._movesAgainstThisPlayer.Contains(pos);
        }

        private static Ship.Ship GetValidShipOrNull(Player targetPlayer, Pos pos) {
            // Location is out of bounds
            if (!CheckIfPosInBoard(targetPlayer, pos)) {
                return null;
            }

            foreach (var ship in targetPlayer._ships) {
                if (!Ship.Ship.IsShipAtPos(ship, pos)) {
                    continue;
                }

                if (Ship.Ship.CanAttackShipAtPos(ship, pos)) {
                    return ship;
                }
            }

            return null;
        }

        private bool CheckIfValidPlacementPos(Pos pos, int shipSize, ShipDirection direction) {
            // Find ship's furthest point
            var maxPos = new Pos(
                pos.X + (direction == ShipDirection.Right ? shipSize : 0),
                pos.Y + (direction == ShipDirection.Right ? 0 : shipSize)
            );

            // Check if ship would be off the board
            if (!CheckIfPosInBoard(this, maxPos)) {
                return false;
            }

            // Check if any ships already exist at that location
            foreach (var ship in _ships) {
                if (Ship.Ship.CheckIfIntersect(ship, pos, shipSize, direction)) {
                    return false;
                }
            }

            return true;
        }

        public bool IsAlive() {
            // If flag is flipped return immediately instead of counting ship blocks
            if (!_isAlive) {
                return false;
            }

            // Count each ship block individually, checking if there is at least one that's not hit
            foreach (var ship in _ships) {
                if (!Ship.Ship.IsDestroyed(ship)) {
                    return true;
                }
            }

            return _isAlive = false;
        }
    }
}