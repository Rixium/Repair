using System;

namespace Repair.Games
{
    public class WorldGenerator
    {

        public static Tile[,] Generate(int width, int height)
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
                    if (dryness > 0.2f)
                    {
                        dryness = 1;
                    }
                    
                    tiles[x, y] = new Tile()
                    {
                        Dryness = dryness
                    };
                    
                }
            }

            return tiles;
        }
        
    }
}