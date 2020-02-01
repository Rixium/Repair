using System;
using Microsoft.Xna.Framework.Graphics;
using Repair.Games;
using Repair.UI;

namespace Repair.Screen
{
    public class GameScreen : IScreen
    {
        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }
        public Action<string> RequestNotification { get; set; }
        public UIManager UIManager { get; set; }

        private World _world;

        public GameScreen()
        {
            _world = new World
            {
                RequestNotification = s => RequestNotification?.Invoke(s)
            };

            UIManager = _world.UIManager;
        }
        
        public void Update(float delta)
        {
            _world.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _world.Draw(spriteBatch);   
        }
    }
}