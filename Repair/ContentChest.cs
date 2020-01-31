using Microsoft.Xna.Framework.Content;

namespace Repair
{
    public class ContentChest
    {
        
        private static ContentManager _contentManager;

        private ContentChest()
        {
            
        }
        
        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        
        public static void Load()
        {
            
        }
        
    }
}