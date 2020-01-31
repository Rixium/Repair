using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Repair.Screen
{
    public class MainMenuScreen : IScreen
    {
        public Action<IScreen> RequestScreenChange { get; set; }

        public MainMenuScreen()
        {
            MediaPlayer.Play(ContentChest.MainMusic);    
        }
        
        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(ContentChest.TitleFont, "Repair", new Vector2(20, 20), new Color(119, 221, 119));
            spriteBatch.End();
        }
        
    }
}