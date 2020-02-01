using System;

namespace Repair.Util
{
    public class Randomizer
    {

        private static Random _random;

        public static void Initialize(int seed)
        {
            _random = new Random(seed);
        }

        public static int RandomMinMax(int min, int max) => _random.Next(min, max);
        
    }
}