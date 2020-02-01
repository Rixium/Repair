using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Notify
{
    public class Notification : INotification
    {

        private Vector2 LeftPosition;
        private Vector2 RightPosition;
        private Rectangle MidRectangle;
        private Vector2 TextPosition;
        
        private const int ScreenPadding = 20;
        private const int TextPadding = 20;
        private float _fade = 1;

        private int _y = ScreenProperties.ScreenHeight;
        private int Y
        {
            set
            {
                LeftPosition.Y = value;
                RightPosition.Y = value;
                MidRectangle.Y = value;
                TextPosition.Y = value;
                _y = value;
            }
            get { return _y; }
        }

        private Vector2 _notifyTextSize;
        private string _text;
        private float _waited;

        public Notification(string text)
        {
            _text = text;
            _notifyTextSize = ContentChest.ButtonFont.MeasureString(_text);
            
            LeftPosition = new Vector2(ScreenProperties.ScreenWidth - ScreenPadding - ContentChest.NotifyRight.Width - TextPadding -_notifyTextSize.X - TextPadding - ContentChest.NotifyLeft.Width, Y);
            RightPosition = new Vector2(ScreenProperties.ScreenWidth - ScreenPadding - ContentChest.NotifyRight.Width, Y);
            MidRectangle = new Rectangle((int) (ScreenProperties.ScreenWidth - ScreenPadding - ContentChest.NotifyRight.Width - TextPadding - _notifyTextSize.X - TextPadding), Y, (int) _notifyTextSize.X + TextPadding * 2, ContentChest.NotifyMid.Height);
            TextPosition = new Vector2(ScreenProperties.ScreenWidth - ScreenPadding - ContentChest.NotifyRight.Width - TextPadding - _notifyTextSize.X, Y);
        }


        public void Update(float delta)
        {
            if (Y <= ScreenProperties.ScreenHeight - MidRectangle.Height - ScreenPadding)
            {
                Y = ScreenProperties.ScreenHeight - MidRectangle.Height - ScreenPadding;

                if (_waited >= 1)
                {
                    _fade -= 2f * delta;
                }
                else
                {
                    _waited += delta;
                }
            }
            else
            {
                Y -= 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentChest.NotifyLeft, LeftPosition, Color.White * _fade);
            spriteBatch.Draw(ContentChest.NotifyMid, MidRectangle, Color.White * _fade);
            spriteBatch.Draw(ContentChest.NotifyRight, RightPosition, Color.White * _fade);
            spriteBatch.DrawString(ContentChest.ButtonFont, _text, TextPosition, Color.White * _fade);
        }
    }
}