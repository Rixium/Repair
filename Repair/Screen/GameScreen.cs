using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Games;
using Repair.Transition;
using Repair.UI;

namespace Repair.Screen
{
    public class GameScreen : IScreen
    {
        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }
        public Action<string> RequestNotification { get; set; }
        public UIManager UIManager { get; set; }
        public Color BackColor { get; set; } = new Color(56, 90, 113);

        public ITransition Transition = new FadeTransition(true);
        
        private World _world;

        public GameScreen(int level = 1)
        {
            _world = new World(level)
            {
                RequestNotification = s => RequestNotification?.Invoke(s),
                RequestTransitionReset = ResetTransition,
                RequestScreenChange = (screen) => RequestScreenChange?.Invoke(screen)
            };

            Transition.OnTransitionOutEnded = _world.Progress;

            UIManager = _world.UIManager;
        }

        private void ResetTransition()
        {
            Transition.Reset();
        }

        public void Update(float delta)
        {
            _world.Update(delta);

            if (!Transition.HasEnded())
            {
                Transition.Update(delta);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _world.Draw(spriteBatch);

            spriteBatch.Begin();
            if (!Transition.HasEnded())
            {
                Transition.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}