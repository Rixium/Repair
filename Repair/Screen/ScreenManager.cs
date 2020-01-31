using Microsoft.Xna.Framework.Graphics;
using Repair.Transition;

namespace Repair.Screen
{
    public class ScreenManager
    {
        
        private readonly ITransition _transition;
        
        private IScreen _activeScreen;

        public ScreenManager()
        {
            _transition = new FadeTransition();
        }

        public void SetScreen(IScreen screen)
        {
            _transition.OnTransitionOutEnded = () =>
            {
                _activeScreen = screen;
            };
            
            _transition.Reset();
        }
        
        public void Update(float delta)
        {
            if (!_transition.HasEnded())
            {
                _transition.Update(delta);
                return;
            }

            _activeScreen?.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeScreen?.Draw(spriteBatch);

            if (_transition.HasEnded()) return;
            spriteBatch.Begin();
            _transition.Draw(spriteBatch);
            spriteBatch.End();
        }
        
    }
}