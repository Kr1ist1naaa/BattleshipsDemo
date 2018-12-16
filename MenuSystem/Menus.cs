using System.Collections.Generic;
using Domain.DomainRule;
using GameSystem;

namespace MenuSystem {
    public static class Menus {
        private static readonly Menu ShipsSizeRulesMenu = new Menu {
            Title = "Ship size rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.ShipSizeRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetShipSizesToDefault,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCarrier).Description,
                    RuleTypeToChange = RuleType.SizeCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeBattleship).Description,
                    RuleTypeToChange = RuleType.SizeBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeSubmarine).Description,
                    RuleTypeToChange = RuleType.SizeSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizeCruiser).Description,
                    RuleTypeToChange = RuleType.SizeCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.SizePatrol).Description,
                    RuleTypeToChange = RuleType.SizePatrol,
                    Shortcut = "F"
                }
            }
        };

        private static readonly Menu ShipCountRulesMenu = new Menu {
            Title = "Ship count rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.ShipCountRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetShipCountsToDefault,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCarrier).Description,
                    RuleTypeToChange = RuleType.CountCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountBattleship).Description,
                    RuleTypeToChange = RuleType.CountBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountSubmarine).Description,
                    RuleTypeToChange = RuleType.CountSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountCruiser).Description,
                    RuleTypeToChange = RuleType.CountCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.CountPatrol).Description,
                    RuleTypeToChange = RuleType.CountPatrol,
                    Shortcut = "F"
                }
            }
        };

        private static readonly Menu GeneralRulesMenu = new Menu {
            Title = "General rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.GeneralRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = Rules.ResetGeneralToDefault,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.PlayerCount).Description,
                    RuleTypeToChange = RuleType.PlayerCount,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.BoardSize).Description,
                    RuleTypeToChange = RuleType.BoardSize,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = Rules.GetRule(RuleType.ShipPadding).Description,
                    RuleTypeToChange = RuleType.ShipPadding,
                    Shortcut = "D"
                }
            }
        };

        private static readonly Menu MainRulesMenu = new Menu {
            Title = "Rules Menu",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.MainRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset everything to default",
                    ActionToExecute = Rules.ResetAllToDefault,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = "List all rules",
                    Shortcut = "B",
                    ActionToExecute = DynamicMenus.PrintAllRules
                },
                new MenuItem {
                    Description = "Change general rules",
                    Shortcut = "C",
                    MenuToRun = GeneralRulesMenu.RunMenu
                },
                new MenuItem {
                    Description = "Change number of ships",
                    Shortcut = "D",
                    MenuToRun = ShipCountRulesMenu.RunMenu
                },
                new MenuItem {
                    Description = "Change size of ships",
                    Shortcut = "E",
                    MenuToRun = ShipsSizeRulesMenu.RunMenu
                }
            }
        };

        private static readonly Menu NewGameMenu = new Menu {
            Title = "Creating new game",
            MenuTypes = new List<MenuType> {MenuType.GameMenu, MenuType.NewGameMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    IsDefaultChoice = true,
                    Description = "Start game",
                    ActionToExecute = GameSystem.GameSystem.NewGame,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = "Change rules",
                    MenuToRun = MainRulesMenu.RunMenu,
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
                    Description = "New Game",
                    MenuToRun = NewGameMenu.RunMenu,
                    ActionToExecute = Rules.ResetAllToDefault,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = "Load save",
                    ActionToExecute = DynamicMenus.LoadGame,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = "Delete save",
                    ActionToExecute = DynamicMenus.DeleteGame, 
                    Shortcut = "C"
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