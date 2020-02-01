using System;
using Microsoft.Xna.Framework.Graphics;
using Repair.Games;

namespace Repair.Screen
{
    public class GameScreen : IScreen
    {
        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }

        private World _world;

        public GameScreen()
        {
            _world = new World();
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