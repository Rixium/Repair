using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.UI;

namespace Repair.Screen
{
    public class MainMenuScreen : IScreen
    {
        public Action<IScreen> RequestScreenChange { get; set; }

        private const int ButtonPadding = 10;
        
        private int _activeButton;
        private Button[] _buttons;

        public MainMenuScreen()
        {
            #if !DEBUG
            MediaPlayer.Play(ContentChest.MainMusic);
            #endif
            
            var startButton = new Button(ContentChest.ButtonFont, "Start", Color.Black, new Vector2(20, 100), Origin.Center);
            var quitButton = new Button(ContentChest.ButtonFont, "Quit", Color.Black, new Vector2(20, startButton.Bottom + ButtonPadding), Origin.Center);
            
            _buttons = new[]
            {
                startButton,
                quitButton
            };
        }
        
        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(ContentChest.TitleFont, "Repair", new Vector2(20, 20), new Color(119, 221, 119));
            
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.End();
        }
        
    }
}