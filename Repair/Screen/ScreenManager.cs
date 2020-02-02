using Microsoft.Xna.Framework;
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

        public bool IsReady => _transition.HasEnded() && _activeScreen != null;
        public bool ShouldUpdateInputManager => _activeScreen != null && _activeScreen.ShouldUpdateInputManager;

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
            
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _activeScreen?.UIManager?.Draw(spriteBatch);

            if (!_transition.HasEnded())
            {
                _transition.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void ResetTransition()
        {
            _transition.Reset();
        }

        public Color GetBackColor() => _activeScreen?.BackColor ?? Color.White;
    }
}