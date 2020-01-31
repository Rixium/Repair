using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Screen
{
    public class ScreenManager
    {

        private IScreen _activeScreen;
        private IScreen _nextScreen;

        public void SetScreen(IScreen screen)
        {
            _nextScreen = screen;
        }
        
        public void Update(GameTime gameTime)
        {
            if (_activeScreen == null)
            {
                _activeScreen = _nextScreen;
            }
            
            _activeScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeScreen.Draw(spriteBatch);
        }
        
    }
}