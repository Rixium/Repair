using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Helpers;
using Repair.Util;

namespace Repair.Games
{
    public class Map
    {
        private readonly Queue<WorldObject> _objectQueue = new Queue<WorldObject>();

        private int MapWidth { get; set; }

        private int MapHeight { get; set; }
        
        public int TileSize { get; set; } = 24;
        public Action<string> RequestNotification { get; set; }
        
        private readonly Tile[,] _tiles;
        private float _totalDryness;

        private Animation WaterEdgeAnimation;
        private Animation WaterAnimation;

        private MapInformation MapInformation;

        public Map(MapData mapData)
        {
            MapWidth = mapData.Width;
            MapHeight = mapData.Height;

            MapInformation = WorldGenerator.Create(this, mapData);
            _tiles = MapInformation.Tiles;
            
            CalculateDryness();
            CreateAnimations();
        }

        public Tile GetPlayerStartingTile() => MapInformation.Start;

        private void CreateAnimations()
        {
            WaterEdgeAnimation = new Animation(ContentChest.WaterEdgeFrames, 1);
        }

        private void CalculateDryness()
        {
            foreach (var tile in _tiles)
            {
                _totalDryness += tile.Dryness;
            }
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _tiles.GetLength(0) || y >= _tiles.GetLength(1)) return null;
            
            return _tiles[x, y];
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var tileStartX = (int)(camera.X - ScreenProperties.ScreenWidth / 3.0f) / TileSize - 1;
            var tileStartY = (int)(camera.Y - ScreenProperties.ScreenHeight / 3.0f) / TileSize - 1;
            var tileEndX = (int)(camera.X + ScreenProperties.ScreenWidth / 3.0f) / TileSize + 1;
            var tileEndY = (int)(camera.Y + ScreenProperties.ScreenHeight / 3.0f) / TileSize + 1;
            
            tileStartX = MathHelper.Clamp(tileStartX, 0, MapWidth);
            tileEndX = MathHelper.Clamp(tileEndX, 0, MapWidth);
            tileStartY = MathHelper.Clamp(tileStartY, 0, MapHeight);
            tileEndY = MathHelper.Clamp(tileEndY, 0, MapHeight);
            
            for (var i = tileStartX; i < tileEndX; i++)
            {
                for (var j = tileStartY; j < tileEndY; j++)
                {
                    if (_tiles[i, j].IsDry)
                    {
                        var tilePos = RenderHelper.CalculateNeighbourBit(_tiles[i, j]);
                        var x = tilePos % 4;
                        var y = tilePos / 4; 
                        
                        spriteBatch.Draw(ContentChest.Grass,
                                new Vector2(i * TileSize, j * TileSize), new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), Color.White);
                    }
                    else
                    {
                        var tileTop = _tiles[i, j].North;
                        if (tileTop != null && tileTop.IsDry)
                        {
                            spriteBatch.Draw(WaterEdgeAnimation.Current, new Vector2(i * TileSize, j * TileSize),
                                Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(ContentChest.Water, new Vector2(i * TileSize, j * TileSize), Color.White);
                        }
                    }

                    if (_tiles[i, j].DroppedItem != null)
                    {
                        var item = _tiles[i, j].DroppedItem;
                        spriteBatch.Draw(ContentChest.Items[item.FileName], new Rectangle(i * TileSize, j * TileSize, TileSize, TileSize), Color.White);
                    }
                    if (_tiles[i, j].WorldObject != null)
                    {
                        _objectQueue.Enqueue(_tiles[i, j].WorldObject);
                    }
                }
            }

            while (_objectQueue.Count > 0)
            {
                var obj = _objectQueue.Dequeue();
                var image = ContentChest.WorldObjects[$"{obj.FileName[obj.Stage - 1]}"];

                var objectOrigin = obj.Origins[obj.Stage - 1];
                var origin = new Vector2(objectOrigin.X, objectOrigin.Y);
                
                var drawPosition = new Vector2(obj.Tile.X * TileSize + TileSize / 2,
                    obj.Tile.Y * TileSize + TileSize / 2);
                
                spriteBatch.Draw(image, drawPosition, null, Color.White, 0, origin, 1,SpriteEffects.None, 0f);
            }
            
        }

        public void Update(float delta)
        {
            WaterEdgeAnimation.Update(delta);
        }

        public Vector2 GetTilePositionVector(Tile tile) => new Vector2(tile.X * TileSize, tile.Y * TileSize);

        public Tile GetTileAtVector(float x, float y)
        {
            var tileX = (int) x / TileSize;
            var tileY = (int) y / TileSize;
            return GetTileAt(tileX, tileY);
        }

        public Tile GetRandomDryTile()
        {
            Tile tile = null;

            do
            {
                var randomX = Randomizer.RandomMinMax(0, MapWidth);
                var randomY = Randomizer.RandomMinMax(0, MapHeight);
                tile = GetTileAt(randomX, randomY);
            } while (tile == null || !tile.IsDry);

            return tile;
        }

        public Tile GetRandomTileInRadius(Tile tile, int radius)
        {
            var randomTileX = tile.X;
            var randomTileY = tile.Y;

            while ((randomTileX == tile.X && randomTileY == tile.Y) || GetTileAt(randomTileX, randomTileY) == null 
                   || GetTileAt(randomTileX, randomTileY).WorldObject != null || GetTileAt(randomTileX, randomTileY).DroppedItem != null
                   || !GetTileAt(randomTileX, randomTileY).IsDry) {
                randomTileX = Randomizer.RandomMinMax(tile.X - radius, tile.X + radius);
                randomTileY = Randomizer.RandomMinMax(tile.Y - radius, tile.Y + radius);
            }
            
            return GetTileAt(randomTileX, randomTileY);
        }

        public IEnumerable<Item> GetStartingItems() => MapInformation.StartingItems;
    }
}