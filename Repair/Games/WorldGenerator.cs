using System;
using System.Collections.Generic;

namespace Repair.Games
{
    public class WorldGenerator
    {
        private const int Starting = 2;
        private const int Seed = 3;
        private const int Cog = 5;
        private const int Turbine = 4;

        public static MapInformation Create(Map map, MapData mapData)
        {
            var tiles = new Tile[mapData.Width, mapData.Height];
            Tile startingTile = null;
            var worldObjects = new List<WorldObject>();
            
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

                if (mapData.Data[i] == Cog)
                {
                    tiles[x, y].DroppedItem = new DroppedItem()
                    {
                        ItemName = "Cog",
                        FileName = "cog",
                        Tile = tiles[x, y],
                        Usable = false,
                        RepairID = 1
                    };
                }
                if (mapData.Data[i] == Turbine)
                {
                     ContentChest.ProtoTypes["turbine1"].CreateInstance(tiles[x, y]);
                     worldObjects.Add(tiles[x, y].WorldObject);
                }
            }

            return new MapInformation()
            {
                Tiles = tiles,
                Start = startingTile,
                StartingItems = mapData.StartingInventory,
                WorldObjects = worldObjects
            };
        }
    }
}