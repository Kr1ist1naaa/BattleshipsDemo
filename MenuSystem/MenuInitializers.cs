using System.Collections.Generic;
using Domain;
using GameSystem;

namespace MenuSystem {
    public static class MenuInitializers {
        private static readonly Menu ShipsSizeRulesMenu = new Menu {
            Title = "Ship size rules",
            MenuTypes = new List<MenuType> {MenuType.RulesMenu, MenuType.ShipSizeRulesMenu},
            MenuItems = new List<MenuItem> {
                new MenuItem {
                    Description = "Reset to default",
                    ActionToExecute = ActiveGame.ResetShipSizeRules,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.SizeCarrier).Description,
                    RuleTypeToChange = RuleType.SizeCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.SizeBattleship).Description,
                    RuleTypeToChange = RuleType.SizeBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.SizeSubmarine).Description,
                    RuleTypeToChange = RuleType.SizeSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.SizeCruiser).Description,
                    RuleTypeToChange = RuleType.SizeCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.SizePatrol).Description,
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
                    ActionToExecute = ActiveGame.ResetShipCountRules,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.CountCarrier).Description,
                    RuleTypeToChange = RuleType.CountCarrier,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.CountBattleship).Description,
                    RuleTypeToChange = RuleType.CountBattleship,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.CountSubmarine).Description,
                    RuleTypeToChange = RuleType.CountSubmarine,
                    Shortcut = "D"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.CountCruiser).Description,
                    RuleTypeToChange = RuleType.CountCruiser,
                    Shortcut = "E"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.CountPatrol).Description,
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
                    ActionToExecute = ActiveGame.ResetGeneralRules,
                    IsResetRules = true,
                    Shortcut = "A"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.PlayerCount).Description,
                    RuleTypeToChange = RuleType.PlayerCount,
                    Shortcut = "B"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.BoardSize).Description,
                    RuleTypeToChange = RuleType.BoardSize,
                    Shortcut = "C"
                },
                new MenuItem {
                    Description = ActiveGame.GetRule(RuleType.ShipPadding).Description,
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
                    ActionToExecute = ActiveGame.ResetAllRules,
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
                    ActionToExecute = ConsoleGame.NewGame,
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
                    ActionToExecute = ActiveGame.ResetAllRules,
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
        
        public static readonly MenuItem ExitProgramItem = new MenuItem {
            Shortcut = "X",
            Description = "Exit program!"
        };
    }
}