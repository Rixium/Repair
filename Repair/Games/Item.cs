namespace Repair.Games
{
    public class Item
    {
        
        public string ItemName { get; set; }
        public string FileName { get; set; }
        public bool Usable { get; set; } = true;
        
        public int RepairID { get; set; }
        public int Count { get; set; }
    }
}