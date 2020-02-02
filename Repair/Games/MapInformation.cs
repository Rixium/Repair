using System.Collections.Generic;

namespace Repair.Games
{
    public class MapInformation
    {
        public Tile[,] Tiles { get; set; }
        public Tile Start { get; set; }
        public Item[] StartingItems { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        
    }
}