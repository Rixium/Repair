using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Helpers;
using Repair.Util;

namespace Repair.Games
{
    public class Map
    {
        
        public int MapWidth { get; set; }

        public int MapHeight { get; set; }
        
        public int TileSize { get; set; } = 24;
        public Action<string> RequestNotification { get; set; }
        
        private readonly Tile[,] _tiles;
        private float _totalDryness;

        private Animation WaterEdgeAnimation;
        private Animation WaterAnimation;

        public Map(int mapWidth, int mapHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            
            _tiles = WorldGenerator.Generate(this, mapWidth, mapHeight);
            CalculateDryness();
            CreateAnimations();
        }

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
            var tileStartX = (int)(camera.X - ScreenProperties.ScreenWidth / 4.0f) / TileSize - 1;
            var tileStartY = (int)(camera.Y - ScreenProperties.ScreenHeight / 4.0f) / TileSize - 1;
            var tileEndX = (int)(camera.X + ScreenProperties.ScreenWidth / 4.0f) / TileSize + 1;
            var tileEndY = (int)(camera.Y + ScreenProperties.ScreenHeight / 4.0f) / TileSize + 1;
            
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
                        var imageName = RenderHelper.CreateNeighborString(_tiles[i, j]);
                        ContentChest.Grass.TryGetValue($"grass_{imageName}", out var texture);
                        if (texture == null) texture = ContentChest.Grass["grass_"];
                        spriteBatch.Draw(texture,
                                new Vector2(i * TileSize, j * TileSize), Color.White);
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
                }
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
    }
}