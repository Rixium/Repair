using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Input;

namespace Repair.Games
{
    public class World
    {

        public Action<string> RequestNotification { get; set; }
        public Map Map { get; }
        private int _xOffset;
        private int _yOffset;

        public World()
        {
            Map = new Map();

            InputManager.OnDownHeld = () => Scroll(0, -1);
            InputManager.OnUpHeld = () => Scroll(0, 1);
            InputManager.OnLeftHeld = () => Scroll(1, 0);
            InputManager.OnRightHeld = () => Scroll(-1, 0);

            Map.RequestNotification = s => RequestNotification?.Invoke(s);
        }

        private void Scroll(int x, int y)
        {
            _xOffset += x * 3;
            _yOffset += y * 3;
        }

        public void Update(float delta)
        {
            Map.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Map.Draw(spriteBatch, _xOffset, _yOffset);
            
            spriteBatch.End();
        }
    }

}