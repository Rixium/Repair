using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Input;
using Repair.Util;

namespace Repair.Games
{
    public class World
    {

        private Camera _camera;
        
        public Action<string> RequestNotification { get; set; }
        public Map Map { get; }
        
        public World()
        {
            Map = new Map();

            InputManager.OnDownHeld = () => Scroll(0, _camera.ScrollSpeed);
            InputManager.OnUpHeld = () => Scroll(0, -_camera.ScrollSpeed);
            InputManager.OnLeftHeld = () => Scroll(-_camera.ScrollSpeed, 0);
            InputManager.OnRightHeld = () => Scroll(_camera.ScrollSpeed, 0);
            InputManager.OnZoomInPressed = () => _camera.Zoom(1);
            InputManager.OnZoomOutPressed = () => _camera.Zoom(-1);

            Map.RequestNotification = s => RequestNotification?.Invoke(s);
            _camera = new Camera(0, 0);
        }

        private void Scroll(int x, int y)
        {
            _camera.Move(x, y);
        }

        public void Update(float delta)
        {
            Map.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Get());

            Map.Draw(spriteBatch, _camera);
            
            spriteBatch.End();
        }
    }

}