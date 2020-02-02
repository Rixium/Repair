using Microsoft.Xna.Framework;

namespace Repair.Games
{
    public class Tile
    {

        public Map Map;
        public float Dryness { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile North => Map.GetTileAt(X, Y - 1);
        public Tile NorthEast => Map.GetTileAt(X + 1, Y - 1);
        public Tile East => Map.GetTileAt(X + 1, Y);
        public Tile SouthEast => Map.GetTileAt(X + 1, Y + 1);
        public Tile South => Map.GetTileAt(X, Y + 1);
        public Tile SouthWest => Map.GetTileAt(X - 1, Y + 1);
        public Tile NorthWest => Map.GetTileAt(X - 1, Y - 1);
        public Tile West => Map.GetTileAt(X - 1, Y);
        
        public bool IsDry => Dryness >= 0.2;
        public Vector2 WorldPosition => Map.GetTilePositionVector(this);
        public DroppedItem DroppedItem { get; set; }

        public WorldObject WorldObject;
        
    }
}