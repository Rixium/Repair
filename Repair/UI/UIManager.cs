using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Games;

namespace Repair.UI
{
    public class UIManager
    {

        private int _slotSize = 64;
        private int _slotPadding = 20;
        private int _selectedSlot;
        
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
                x += i * (_slotSize + _slotPadding);

                var position = new Rectangle(x, ScreenProperties.ScreenHeight - _slotPadding - _slotSize, _slotSize,
                    _slotSize);

                if (_selectedSlot == i)
                {
                    spriteBatch.Draw(ContentChest.SlotBackground, position,
                        Color.White * 0.8f);
                }
                else
                {
                    spriteBatch.Draw(ContentChest.SlotBackground, position,
                        Color.White * 0.3f);
                }

                if (slot.Item != null)
                {
                    spriteBatch.Draw(ContentChest.Items[slot.Item.FileName], position, Color.White);
                }

                spriteBatch.Draw(ContentChest.SlotBorder, position,
                    Color.White);

                if (slot.Item != null)
                {
                    var text = $"{slot.Count}";
                    var textPos = new Vector2(position.X + position.Width + 4 - ContentChest.SlotFont.MeasureString(text).X, position.Y + position.Height - ContentChest.SlotFont.MeasureString(text).Y + 4);
                    DrawText(spriteBatch, ContentChest.SlotFont, text, Color.Black, Color.White, 1, textPos);
                }

                i++;
            }

            var activeItem = Inventory.Slots[_selectedSlot].Item;

            if (activeItem != null)
            {
                var text = activeItem.ItemName;
                var textSize = ContentChest.SlotFont.MeasureString(text);
                DrawText(spriteBatch, ContentChest.SlotFont,  text, Color.Black, Color.White, 1, new Vector2(ScreenProperties.ScreenWidth / 2.0f - textSize.X / 2, ScreenProperties.ScreenHeight - _slotPadding - _slotSize - _slotPadding - textSize.Y));
            }
        }


        public static void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text, Color backColor, Color frontColor, float scale, Vector2 position)
        {
            var origin = Vector2.Zero;
            spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, 1 * scale), backColor, 0, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, 1 * scale), backColor, 0, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, -1 * scale), backColor, 0, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, -1 * scale), backColor, 0, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position, frontColor, 0, origin, scale, SpriteEffects.None, 0f);
        }

        public void NextSlot()
        {
            _selectedSlot++;
            if (_selectedSlot >= Inventory.Slots.Length)
                _selectedSlot = 0;
        }

        public void LastSlot()
        {
            _selectedSlot--;
            if (_selectedSlot < 0) _ = _selectedSlot = Inventory.Slots.Length - 1;
        }

        public Slot GetSelectedSlot()
        {
            return Inventory.GetSlot(_selectedSlot);
        }
    }
}