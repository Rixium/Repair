using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Input;
using Repair.Util;

namespace Repair.Games
{
    public class World
    {

        public int MapSize { get; set; } = 500;

        private Camera _camera;
        public Action<string> RequestNotification { get; set; }
        public Map Map { get; }
        
        public Player Player { get; set; }
        
        public World()
        {
            Map = new Map(MapSize, MapSize);

            InputManager.OnDownHeld = () => Player.Move(0, Player.Speed);
            InputManager.OnUpHeld = () => Player.Move(0, -Player.Speed);
            InputManager.OnLeftHeld = () => Player.Move(-Player.Speed, 0);
            InputManager.OnRightHeld = () => Player.Move(Player.Speed, 0);
            InputManager.OnZoomInPressed = () => _camera.Zoom(1);
            InputManager.OnZoomOutPressed = () => _camera.Zoom(-1);

            Map.RequestNotification = s => RequestNotification?.Invoke(s);

            var playerStartTile = Map.GetTileAt(MapSize / 2, MapSize / 2);
            var playerStartVector = Map.GetTilePositionVector(playerStartTile);
            Player = new Player(playerStartVector);

            _camera = new Camera((int) playerStartVector.X, (int) playerStartVector.Y);
            _camera.SetFollowTarget(Player);
        }

        private void Scroll(int x, int y)
        {
            _camera.Move(x, y);
        }

        public void Update(float delta)
        {
            Map.Update(delta);
            _camera.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Get());

            Map.Draw(spriteBatch, _camera);
            
            spriteBatch.Draw(ContentChest.Pixel, new Rectangle((int) Player.Position.X, (int) Player.Position.Y, Map.TileSize, Map.TileSize), Color.White);
            spriteBatch.End();
        }
    }

}