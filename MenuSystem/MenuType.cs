namespace MenuSystem {
    public enum MenuType {
        MainMenu, 
        GameMenu, 
            NewGameMenu, LoadGameMenu, DeleteGameMenu,
        RulesMenu,
            MainRulesMenu, GeneralRulesMenu, ShipSizeRulesMenu, ShipCountRulesMenu,
        Input,
            IntInput, StringInput
    }
}