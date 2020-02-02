namespace Repair.Games
{
    public class DroppedItem
    {
        
        public Tile Tile { get; set; }
        
        public string ItemName { get; set; }
        public string FileName { get; set; }
        public bool Usable { get; set; }
        public int RepairID { get; set; }
    }
}