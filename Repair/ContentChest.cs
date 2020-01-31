using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Repair
{
    public static class ContentChest
    {
        
        private static ContentManager _contentManager;

        public static Song MainMusic { get; set; }
        
        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        
        public static void Load()
        {
            MainMusic = _contentManager.Load<Song>("Music/main");
        }
        
    }
}