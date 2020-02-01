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

            InputManager.OnDownHeld = () => Player.Move(0, 1);
            InputManager.OnUpHeld = () => Player.Move(0, -1);
            InputManager.OnLeftHeld = () => Player.Move(-1, 0);
            InputManager.OnRightHeld = () => Player.Move(1, 0);
            InputManager.OnZoomInPressed = () => _camera.Zoom(1);
            InputManager.OnZoomOutPressed = () => _camera.Zoom(-1);

            Map.RequestNotification = s => RequestNotification?.Invoke(s);

            SetupPlayer();
            SetupCamera();
        }

        private void SetupPlayer()
        {
            var playerStartTile = Map.GetRandomDryTile();
            
            Player = new Player(playerStartTile)
            {
                OnTryMove = TryMove
            };
        }
        
        private void SetupCamera()
        {
            _camera = new Camera((int) Player.Tile.WorldPosition.X, (int) Player.Tile.WorldPosition.Y);
            _camera.SetFollowTarget(Player);
        }

        private bool TryMove(Tile tile)
        {
            return tile != null && tile.IsDry;
        }

        private void Scroll(int x, int y)
        {
            _camera.Move(x, y);
        }

        public void Update(float delta)
        {
            Map.Update(delta);
            Player.Update(delta);
            
            _camera.Update(delta);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Get());

            Map.Draw(spriteBatch, _camera);

            DrawPlayer(spriteBatch);
            spriteBatch.End();
        }

        private void DrawPlayer(SpriteBatch spriteBatch)
        {
            var drawVector = Player.Tile.WorldPosition;
            var targetVector = Player.TargetTile.WorldPosition;
            
            drawVector -= (drawVector - targetVector) * Player.MovementPercentage;
            
            spriteBatch.Draw(ContentChest.Pixel, new Rectangle((int) drawVector.X, (int) drawVector.Y, Map.TileSize, Map.TileSize), Color.White);
        }
    }

}