using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Helpers;
using Repair.Notify;
using Repair.Util;

namespace Repair.Games
{
    public class Map
    {
        
        public int TileSize { get; set; } = 24;
        public Action<string> RequestNotification { get; set; }
        
        private readonly Tile[,] _tiles;
        private float _totalDryness;

        public Map()
        {
            _tiles = WorldGenerator.Generate(this, 500, 500);
            CalcualteDryness();
        }

        private void CalcualteDryness()
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
            var tileStartX = (camera.X - ScreenProperties.ScreenWidth) / TileSize - 1;
            var tileStartY = (camera.Y - ScreenProperties.ScreenHeight) / TileSize - 1;
            var tileEndX = (camera.X + ScreenProperties.ScreenWidth) / TileSize + 1;
            var tileEndY = (camera.Y + ScreenProperties.ScreenHeight) / TileSize + 1;
            
            tileStartX = MathHelper.Clamp(tileStartX, 0, _tiles.GetLength(0));
            tileEndX = MathHelper.Clamp(tileEndX, 0, _tiles.GetLength(0));
            tileStartY = MathHelper.Clamp(tileStartY, 0, _tiles.GetLength(1));
            tileEndY = MathHelper.Clamp(tileEndY, 0, _tiles.GetLength(1));
            
            for (var i = tileStartX; i < tileEndX; i++)
            {
                for (var j = tileStartY; j < tileEndY; j++)
                {
                    if (_tiles[i, j].IsDry)
                    {
                        var imageName = RenderHelper.CreateNeighborString(_tiles[i, j]);
                        try
                        {
                            spriteBatch.Draw(ContentChest.Grass[$"grass_{imageName}"],
                                new Vector2(i * TileSize, j * TileSize), Color.White);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(imageName);
                        }
                    }
                    else
                    {
                        var tileTop = _tiles[i, j].North;
                        spriteBatch.Draw(ContentChest.Grass["grass_"], new Vector2(i * TileSize, j * TileSize), Color.White * _tiles[i, j].Dryness);
                        
                        if (tileTop != null && tileTop.IsDry)
                        {
                            spriteBatch.Draw(ContentChest.WaterEdge, new Vector2(i * TileSize, j * TileSize), Color.White * 0.5f);
                        }
                        else
                        {
                            spriteBatch.Draw(ContentChest.Water, new Vector2(i * TileSize, j * TileSize), Color.White * 0.5f);
                        }
                    }
                }
            }
        }

        public void Update(float delta)
        {
            if (_totalDryness > 70000)
            {
                return;
            }
            
            for (var i = 0; i < _tiles.GetLength(0); i++)
            {
                for (var j = 0; j < _tiles.GetLength(1); j++)
                {
                    _totalDryness -= _tiles[i, j].Dryness;
                    _tiles[i, j].Dryness += 0.1f * delta;
                    _totalDryness += _tiles[i, j].Dryness;
                }
            }
            
            if (_totalDryness > 70000)
            {
                RequestNotification?.Invoke("Flood Subsided");
            }
        }

    }
}