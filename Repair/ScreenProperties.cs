using Microsoft.Xna.Framework;

namespace Repair
{
    public static class ScreenProperties
    {

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        
        public static Vector2 Center(int imageWidth = 0, int imageHeight = 0) => 
            new Vector2(ScreenWidth / 2 - imageWidth / 2, ScreenHeight / 2 - imageHeight / 2);
        
    }
}