using System;

namespace Repair.Games
{
    public class WorldGenerator
    {

        public static Tile[,] Generate(Map map, int width, int height)
        {
            var noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            noise.SetFrequency(0.03f);
            
            var tiles = new Tile[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var dryness = noise.GetNoise(x, y);

                    tiles[x, y] = new Tile()
                    {
                        X = x,
                        Y = y,
                        Map = map,
                        Dryness = dryness
                    };
                    
                }
            }

            
            return tiles;
        }

    }
}