using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.UI;

namespace Repair.Screen
{
    public class FinishScreen : IScreen
    {

        public int Padding = 20;
        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }
        public Action<string> RequestNotification { get; set; }
        public UIManager UIManager { get; set; }
        public Color BackColor { get; set; } = new Color(56, 90, 113);
        
        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            var text = "Thank you for playing!";
            var textSize = ContentChest.SlotFont.MeasureString(text);
            var textpos = new Vector2(ScreenProperties.ScreenWidth / 2.0f - textSize.X / 2,
                ScreenProperties.ScreenHeight / 2.0f - textSize.Y / 2 - 100);
            
            UIManager.DrawText(spriteBatch, ContentChest.SlotFont, text, Color.Black, Color.White, 1,
                textpos);

            var text2 = "Programming by Rixium";
            var textSize2 = ContentChest.CreditFont.MeasureString(text2);
            var textPos2 = new Vector2(ScreenProperties.ScreenWidth / 2.0f - textSize2.X / 2,
                textpos.Y + textSize.Y + Padding);
            
            UIManager.DrawText(spriteBatch, ContentChest.CreditFont, text2, Color.Black, Color.White, 1,
                textPos2);
            
            var text3 = "Art by TiffanyJaynexo";
            var textSize3 = ContentChest.CreditFont.MeasureString(text3);
            var textPos3 = new Vector2(ScreenProperties.ScreenWidth / 2.0f - textSize3.X / 2,
                textPos2.Y + textSize2.Y + Padding);
            UIManager.DrawText(spriteBatch, ContentChest.CreditFont, text3, Color.Black, Color.White, 1,
                textPos3);
            
            spriteBatch.End();
        }
        
    }
}