using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.UI;

namespace Repair.Screen
{
    public class SplashScreen : IScreen
    {

        private readonly Rectangle _splashRectangle;

        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }
        public Action<string> RequestNotification { get; set; }
        public UIManager UIManager { get; set; }

        public SplashScreen()
        {
            var splashPosition = ScreenProperties.Center(
                ContentChest.Splash.Width / 2, 
                ContentChest.Splash.Height / 2
                );
            
            _splashRectangle = new Rectangle(
                (int) splashPosition.X, 
                (int) splashPosition.Y, 
                ContentChest.Splash.Width / 2, 
                ContentChest.Splash.Height / 2);
            
            ContentChest.OnLoaded = OnContentLoaded;
        }

        public void Update(float delta)
        {
            ContentChest.Load();
        }

        private void OnContentLoaded()
        {
            RequestScreenChange?.Invoke(new MainMenuScreen());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.Draw(ContentChest.Splash, _splashRectangle, Color.White);
            spriteBatch.End();
        }
    }
}