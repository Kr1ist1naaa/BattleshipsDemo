using System.Collections.Generic;
using Domain.Rule;

namespace MenuSystem {
    public static class Menus {
        public static readonly Menu ShipsSizeRulesMenu = new Menu {
            Title = "Ship size rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.ShipSizeRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetShipSizesToDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCarrier).Description,
                    RuleType = RuleType.SizeCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeBattleship).Description,
                    RuleType = RuleType.SizeBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeSubmarine).Description,
                    RuleType = RuleType.SizeSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCruiser).Description,
                    RuleType = RuleType.SizeCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizePatrol).Description,
                    RuleType = RuleType.SizePatrol,
                    Shortcut = "F"
                }
            }
        };

        public static readonly Menu ShipCountRulesMenu = new Menu {
            Title = "Ship count rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.ShipCountRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetShipCountsToDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCarrier).Description,
                    RuleType = RuleType.CountCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountBattleship).Description,
                    RuleType = RuleType.CountBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountSubmarine).Description,
                    RuleType = RuleType.CountSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCruiser).Description,
                    RuleType = RuleType.CountCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountPatrol).Description,
                    RuleType = RuleType.CountPatrol,
                    Shortcut = "F"
                }
            }
        };

        public static readonly Menu GeneralRulesMenu = new Menu {
            Title = "General rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.GeneralRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetGeneralToDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.PlayerCount).Description,
                    RuleType = RuleType.PlayerCount,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.BoardSize).Description,
                    RuleType = RuleType.BoardSize,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.ShipPadding).Description,
                    RuleType = RuleType.ShipPadding,
                    Shortcut = "D"
                }
            }
        };

        public static readonly Menu RulesMenu = new Menu {
            Title = "Rules Menu",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.MainRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset everything to default",
                    ActionToExecute = Rules.ResetAllToDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = "Change general rules",
                    Shortcut = "B",
                    CommandToExecute = GeneralRulesMenu.RunMenu
                },
                new MenuItem {
                    Description = "Change number of ships",
                    Shortcut = "D",
                    CommandToExecute = ShipCountRulesMenu.RunMenu
                },
                new MenuItem {
                    Description = "Change size of ships",
                    Shortcut = "E",
                    CommandToExecute = ShipsSizeRulesMenu.RunMenu
                }
            }
        };

        public static readonly Menu GameMenu = new Menu {
            Title = "Game Menu",
            MenuTypes = new List<MenuType> {MenuType.GameMenu},
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

        public static readonly Menu MainMenu = new Menu {
            Title = "Main Menu",
            MenuTypes = new List<MenuType> {MenuType.MainMenu},
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
        
        public static readonly MenuItem GoBackItem = new MenuItem {
            Shortcut = "X",
            Description = "Go back!"
        };

        public static readonly MenuItem QuitToMainItem = new MenuItem {
            Shortcut = "Q",
            Description = "Quit to main menu!"
        };
    }
}