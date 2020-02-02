namespace Repair.Games
{
    public class MapData
    {
        public Layer[] Layers { get; set; }
        public int[] Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Item[] StartingInventory { get; set; }
    }
}