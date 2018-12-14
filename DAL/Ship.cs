namespace DAL {
    public class Ship {
        public int ShipId { get; set; }
        public string Title { get; set; }
        public char Symbol { get; set; }
        public int Size { get; set; }
        public int Direction { get; set; }
        
        public int[] ShipStatuses { get; set; }
        public Pos Position { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}