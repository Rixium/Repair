namespace Repair.Games
{
    public class Tile
    {

        public Map Map;
        public float Dryness { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile North => Map.GetTileAt(X, Y - 1);
        public Tile South => Map.GetTileAt(X, Y + 1);
        public Tile East => Map.GetTileAt(X + 1, Y);
        public Tile West => Map.GetTileAt(X - 1, Y);
        public bool IsDry => Dryness >= 0.2;
    }
}