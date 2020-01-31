using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Screen
{
    public class ScreenManager
    {

        private const float FadeSpeed = 0.5f;
        
        private bool _transitioning;
        private float _fade = 1.0f;

        private IScreen _activeScreen;
        private IScreen _nextScreen;

        public void SetScreen(IScreen screen)
        {
            _nextScreen = screen;
            _transitioning = true;
        }
        
        public void Update(float delta)
        {
            if (_transitioning)
            {
                UpdateTransition(delta);
            }
            
            _activeScreen?.Update(delta);
        }

        private void UpdateTransition(float delta)
        {
            if (_nextScreen != null)
            {
                _fade += FadeSpeed * delta;
                
                if (_fade < 1.0f) return;
                
                _fade = 1;
                _activeScreen = _nextScreen; 
                _nextScreen = null;
                
                return;
            }
            
            
            _fade -= FadeSpeed * delta;

            if (_fade > 0) return;
            _fade = 0; 
            _transitioning = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeScreen?.Draw(spriteBatch);

            if (!_transitioning) return;
            
            spriteBatch.Begin();
            spriteBatch.Draw(ContentChest.Pixel, new Rectangle(0, 0, ScreenProperties.ScreenWidth, ScreenProperties.ScreenHeight), Color.Black * _fade);
            spriteBatch.End();
        }
        
    }
}