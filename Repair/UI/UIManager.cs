using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Games;

namespace Repair.UI
{
    public class UIManager
    {

        private int _slotSize = 32;
        private int _slotPadding = 20;
        
        public Inventory Inventory { get; set; }

        public UIManager()
        {
            Inventory = new Inventory();
        }

        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var i = 0;
            foreach (var slot in Inventory.Slots)
            {
                var x = ScreenProperties.ScreenWidth / 2 - _slotSize / 2 - _slotPadding - _slotSize;
                x += i++ * (_slotSize + _slotPadding);
                
                spriteBatch.Draw(ContentChest.Pixel, 
                    new Rectangle(x, ScreenProperties.ScreenHeight - _slotPadding - _slotSize, _slotSize, _slotSize), 
                    Color.Black);
            }
        }
        
    }
}