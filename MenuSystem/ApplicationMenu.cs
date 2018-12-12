using System.Collections.Generic;
using Domain;
using Domain.Rule;

namespace MenuSystem {
    public static class ApplicationMenu {
        public static readonly Menu GameMenu = new Menu {
            Title = "Game Menu",
            IsGameMenu = true,
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    IsDefaultChoice = true,
                    Description = "Start Game",
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = "Select Save & Start Game",
                    Shortcut = "B"
                }
            }
        };

        public static readonly Menu RulesMenu = new Menu {
            Title = "Rules Menu",
            IsRulesMenu = true,
            
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    RuleType = RuleType.ResetDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.BoardSize).Description,
                    RuleType = RuleType.BoardSize,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.ShipPadding).Description,
                    RuleType = RuleType.ShipPadding,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCarrier).Description,
                    RuleType = RuleType.CountCarrier,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountBattleship).Description,
                    RuleType = RuleType.CountBattleship,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountSubmarine).Description,
                    RuleType = RuleType.CountSubmarine,
                    Shortcut = "F"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCruiser).Description,
                    RuleType = RuleType.CountCruiser,
                    Shortcut = "G"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountPatrol).Description,
                    RuleType = RuleType.CountPatrol,
                    Shortcut = "H"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCarrier).Description,
                    RuleType = RuleType.SizeCarrier,
                    Shortcut = "I"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeBattleship).Description,
                    RuleType = RuleType.SizeBattleship,
                    Shortcut = "J"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeSubmarine).Description,
                    RuleType = RuleType.SizeSubmarine,
                    Shortcut = "K"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCruiser).Description,
                    RuleType = RuleType.SizeCruiser,
                    Shortcut = "L"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizePatrol).Description,
                    RuleType = RuleType.SizePatrol,
                    Shortcut = "M"
                }
            }
        };

        public static readonly Menu MainMenu = new Menu {
            Title = "Main Menu",
            IsMainMenu = true,
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    IsDefaultChoice = true,
                    Description = "Game",
                    Shortcut = "A",
                    CommandToExecute = GameMenu.RunMenu
                },
                new MenuItem {
                    Description = "Rules",
                    Shortcut = "B",
                    CommandToExecute = RulesMenu.RunMenu
                }
            } 
        };
    }
}