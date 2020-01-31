using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Screen
{
    public class SplashScreen : IScreen
    {

        private readonly Rectangle _splashRectangle;
        private Timer _timer;

        public Action<IScreen> RequestScreenChange { get; set; }

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

            _timer = new Timer
            {
                Interval = 5000
            };
            
            _timer.Elapsed += (e, b) => OnTimerEnd();

            _timer.Start();
        }

        private void OnTimerEnd()
        {
            _timer.Stop();
            RequestScreenChange?.Invoke(new SplashScreen());
        }

        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ContentChest.Splash, _splashRectangle, Color.White);
            spriteBatch.End();
        }
    }
}