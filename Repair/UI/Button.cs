using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.UI
{
    public class Button
    {
        
        private readonly SpriteFont _font;
        private string _text;
        private readonly Color _color;
        private Vector2 _position;
        private Vector2 _stringMeasurements;

        public Button(SpriteFont font, string text, Color color, Vector2 position, Origin origin)
        {
            _font = font;
            _text = text;
            _color = color;
            _position = position;

            _stringMeasurements = font.MeasureString(text);
            
            Right = new Vector2(_position.X + _stringMeasurements.X, _position.Y);
        }

        public float Bottom => _position.Y + _stringMeasurements.Y;
        public Vector2 Right { get; set; }

        public void Click()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, _position, _color);   
        }
        
    }
}