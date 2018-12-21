using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using GameSystem.Logic;

namespace GameSystem {
    public static class ActiveGame {
        public static HashSet<Rule> RuleSet = RuleLogic.GenDefaultRuleSet();
        public static List<Move> Moves;
        public static List<Player> Players;
        public static int RoundCounter;
        public static int? GameId;
        
        public static Player Winner;
        public static Player CurrentPlayer;
        public static Player NextPlayer;

        static ActiveGame() {
            Init();
        }

        public static void Init() {
            Moves = new List<Move>();
            Winner = null;
            CurrentPlayer = null;
            NextPlayer = null;
            GameId = null;
            Players = new List<Player>();
            RoundCounter = 0;
        }

        public static bool InitPlayerPointers() {
            // No players defined yet
            if (Players == null || Players.Count < 2) {
                return false;
            }
            
            // First run, set default values and return
            CurrentPlayer = Players[0];
            NextPlayer = Players[1];
            
            return true;
        }
        
        public static bool CyclePlayers() {
            // No players defined yet
            if (Players == null || Players.Count < 2) {
                return false;
            }

            // Cycle the players
            CurrentPlayer = NextPlayer;
            NextPlayer = GameLogic.FindNextPlayer(Players, CurrentPlayer);

            // If a round has completed
            if (CurrentPlayer == Players[0]) {
                RoundCounter++;
            }

            return true;
        }
        
        public static bool TrySetWinner() {
            Player winner = null;
            
            // Check if there is only one player left and therefore the winner of the game
            foreach (var player in Players) {
                if (!PlayerLogic.IsAlive(player)) {
                    continue;
                }

                if (winner == null) {
                    winner = player;
                } else {
                    winner = null;
                    break;
                }
            }

            // More than 1 player alive, no winner
            if (winner == null) {
                return false;
            } 
            
            // 1 winner alive
            Winner = winner;
            return true;
        }

        public static bool TryChangeRule(RuleType type, int ruleValue) {
            if (RuleSet == null) {
                throw new Exception("Rules not initalized for game!");
            }
            
            // Get rule
            var rule = RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));

            // No match, couldn't change
            if (rule == null) {
                throw new Exception("Rule not found!");
            }

            // Out of range
            if (ruleValue > rule.MaxVal || ruleValue < rule.MinVal) {
                return false;
            }

            rule.Value = ruleValue;
            return true;
        }
        
        public static int GetRuleVal(RuleType type) {
            // Get rule
            var rule = RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));
            
            // No match
            if (rule == null) {
                throw new Exception("Rule not found!");
            }
            
            return rule.Value;
        }

        public static Rule GetRule(RuleType type) {
            // Get rule
            var rule = RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));
            
            // No match
            if (rule == null) {
                throw new Exception("Rule not found!");
            }
            
            return rule;
        }

        public static void ResetGeneralRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.GeneralRules);
        }

        public static void ResetShipSizeRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.ShipSizeRules);
        }

        public static void ResetShipCountRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.ShipCountRules);
        }
        
        public static void ResetAllRules() {
            ResetGeneralRules();
            ResetShipSizeRules();
            ResetShipCountRules();
        }
    }
}