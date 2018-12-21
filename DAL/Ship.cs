using System.Collections.Generic;

namespace DAL {
    public class Ship {
        public int Id { get; set; }
        public Player Player { get; set; }
        
        public string Title { get; set; }
        public char Symbol { get; set; }
        public int Size { get; set; }
        public int Direction { get; set; }
        
        public List<ShipStatus> ShipStatuses { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}