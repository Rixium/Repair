using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Input;
using Repair.UI;
using Repair.Util;

namespace Repair.Games
{
    public class World
    {

        private List<WorldObject> WorldObjects = new List<WorldObject>(); 
        public UIManager UIManager;
        public int MapSize { get; set; } = 1000;

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
            InputManager.OnNextSlotPressed = () => UIManager.NextSlot();
            InputManager.OnLastSlotPressed = () => UIManager.LastSlot();
            InputManager.OnInteractPressed = UseItem;
            InputManager.OnPickupPressed = PickupWorldObject;
            
            Map.RequestNotification = s => RequestNotification?.Invoke(s);

            UIManager = new UIManager();
            
            SetupPlayer();
            SetupCamera();
        }

        private void PickupWorldObject()
        {
            if (Player.TargetTile != Player.Tile) return;
            
            var facingTile = Player.GetFacingTile();
            var obj = facingTile?.WorldObject;
            if (obj == null) return;
            if (!obj.CanPickup) return;
            var item = new Item()
            {
                FileName = obj.FileName[0],
                ItemName = obj.ObjectName[0],
                Usable = true
            };

            var successful = UIManager.Inventory.AddItem(item, 1);

            if (!successful) return;
            
            WorldObjects.Remove(obj);
            facingTile.WorldObject = null;
        }

        private void UseItem()
        {
            if (Player.TargetTile != Player.Tile) return;
            
            var slot = UIManager.GetSelectedSlot();
            if (slot.Item == null) return;
            if (!slot.Item.Usable) return;

            var protoType = ContentChest.ProtoTypes[slot.Item.FileName];
            var successful = protoType.CreateInstance(Player.GetFacingTile());
            if (!successful) return;
            
            slot.Remove(1);
            AddWorldObject(Player.GetFacingTile().WorldObject);
        }

        private void AddWorldObject(WorldObject worldObject)
        {
            WorldObjects.Add(worldObject);
        }

        private void SetupPlayer()
        {
            var playerStartTile = Map.GetRandomDryTile();
            
            Player = new Player(playerStartTile, CreateStartingInventory())
            {
                OnTryMove = TryMove
            };

            UIManager.Inventory = Player.Inventory;
        }

        private static Inventory CreateStartingInventory()
        {
            var inventory = new Inventory();
            
            inventory.AddItem(new Item()
            {
                ItemName = "Tent",
                FileName = "tent",
                Usable = true
            });
            
            inventory.AddItem(new Item()
            {
                ItemName = "Seed",
                FileName = "seed",
                Usable = true
            }, 5);

            return inventory;
        }

        private void SetupCamera()
        {
            _camera = new Camera((int) Player.Tile.WorldPosition.X, (int) Player.Tile.WorldPosition.Y);
            _camera.SetFollowTarget(Player);
        }

        private bool TryMove(Tile tile)
        {
            if (tile == null) return false;
            if (tile.WorldObject != null && tile.WorldObject.Collidable) return false;
            return tile.IsDry;
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