using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Games
{
    public class Map
    {
        private readonly Tile[,] _tiles;

        public Map(Tile[,] tiles)
        {
            _tiles = tiles;
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            var tileStartX = -(xOffset / 24);
            var tileEndX = -(xOffset / 24) + (ScreenProperties.ScreenWidth / 24) + 2;
            var tileStartY = -(yOffset / 24);
            var tileEndY = -(yOffset / 24) + (ScreenProperties.ScreenHeight / 24) + 2;

            tileStartX = MathHelper.Clamp(tileStartX, 0, _tiles.GetLength(0));
            tileEndX = MathHelper.Clamp(tileEndX, 0, _tiles.GetLength(0));
            tileStartY = MathHelper.Clamp(tileStartY, 0, _tiles.GetLength(1));
            tileEndY = MathHelper.Clamp(tileEndY, 0, _tiles.GetLength(1));
            
            for (var i = tileStartX; i < tileEndX; i++)
            {
                for (var j = tileStartY; j < tileEndY; j++)
                {
                    if (_tiles[i, j].Dryness >= 0.9)
                    {
                        spriteBatch.Draw(ContentChest.Grass, new Vector2(i * 24 + xOffset, j * 24 + yOffset), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(ContentChest.Grass, new Vector2(i * 24 + xOffset, j * 24 + yOffset), Color.White * _tiles[i, j].Dryness);
                        spriteBatch.Draw(ContentChest.Water, new Vector2(i * 24 + xOffset, j * 24 + yOffset), Color.White * 0.5f);
                    }
                }
            }
        }

        public void Update(float delta)
        {
        }

    }
}