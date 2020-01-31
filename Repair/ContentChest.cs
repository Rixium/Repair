using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Repair
{
    public static class ContentChest
    {

        private static bool _loadInitialized;
        public static Action OnLoaded;
        
        private static ContentManager _contentManager;

        public static Texture2D Pixel { get; set; }
        public static Texture2D Splash { get; set; }
        public static Song MainMusic { get; set; }
        public static SpriteFont TitleFont { get; set; }
        public static SpriteFont ButtonFont { get; set; }
        
        public static Texture2D NotifyLeft { get; set; }
        public static Texture2D NotifyRight { get; set; }
        public static Texture2D NotifyMid { get; set; }

        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static void BasicLoad()
        {            
            ButtonFont = _contentManager.Load<SpriteFont>("Fonts/button");
            NotifyLeft = _contentManager.Load<Texture2D>("Images/notify_left");
            NotifyRight = _contentManager.Load<Texture2D>("Images/notify_right");
            NotifyMid = _contentManager.Load<Texture2D>("Images/notify_mid");
            Pixel = _contentManager.Load<Texture2D>("Images/pixel");
            Splash = _contentManager.Load<Texture2D>("Images/splash");
        }
        
        public static void Load()
        {
            if (_loadInitialized) return;
            
            _loadInitialized = true;
            
            TitleFont = _contentManager.Load<SpriteFont>("Fonts/title");
            MainMusic = _contentManager.Load<Song>("Music/main");
            
            OnLoaded?.Invoke();
        }
        
    }
}