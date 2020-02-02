using System;

namespace Repair.Games
{
    public class WorldGenerator
    {
        private const int Starting = 2;
        private const int Seed = 3;
        private const int GearBox = 4;
        private const int Turbine = 5;

        public static MapInformation Create(Map map, MapData mapData)
        {
            var tiles = new Tile[mapData.Width, mapData.Height];
            Tile startingTile = null;
            
            for (var i = 0; i < mapData.Width * mapData.Height; i++)
            {
                var x = i % mapData.Width;
                var y = i / mapData.Width;
                
                tiles[x, y] = new Tile()
                {
                    X = x,
                    Y = y,
                    Map = map,
                    Dryness = mapData.Data[i]
                };

                if (mapData.Data[i] == Starting)
                {
                    startingTile = tiles[x, y];
                }

                if (mapData.Data[i] == Seed)
                {
                    tiles[x, y].DroppedItem = new DroppedItem()
                    {
                        ItemName = "Seed",
                        FileName = "seed",
                        Tile = tiles[x, y],
                        Usable = true
                    };
                }
            }

            return new MapInformation()
            {
                Tiles = tiles,
                Start = startingTile,
                StartingItems = mapData.StartingInventory
            };
        }
    }
}