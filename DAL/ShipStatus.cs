namespace DAL {
    public class ShipStatus {
        public int Id { get; set; }
        public Ship Ship { get; set; }
        
        public int Status { get; set; }
        public int Offset { get; set; }
    }
}