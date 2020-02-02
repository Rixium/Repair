using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.Input;
using Repair.Screen;
using Repair.UI;
using Repair.Util;

namespace Repair.Games
{
    public class World
    {

        public Action<IScreen> RequestScreenChange { get; set; }
        public Action RequestTransitionReset { get; set; }
        
        private List<WorldObject> WorldObjects = new List<WorldObject>(); 
        public UIManager UIManager;
        public int MapSize { get; set; } = 300;

        private Camera _camera;
        public Action<string> RequestNotification { get; set; }
        public Map Map { get; }
        private int _currentLevel = 1;
        public Player Player { get; set; }

        private Timer ProgressTimer;
        
        public World()
        {
            Map = new Map(ContentChest.Maps[_currentLevel]);

            InputManager.OnDownHeld = () => Player.Move(0, 1);
            InputManager.OnUpHeld = () => Player.Move(0, -1);
            InputManager.OnLeftHeld = () => Player.Move(-1, 0);
            InputManager.OnRightHeld = () => Player.Move(1, 0);
            InputManager.OnNextSlotPressed = () => UIManager.NextSlot();
            InputManager.OnLastSlotPressed = () => UIManager.LastSlot();
            InputManager.OnInteractPressed = UseItem;
            InputManager.OnPickupPressed = PickupWorldObject;
            InputManager.OnBackPressed = ResetWorld;
            
            Map.RequestNotification = s => RequestNotification?.Invoke(s);

            UIManager = new UIManager();
            
            SetupPlayer();
            SetupCamera();

            ProgressTimer = new Timer();
            ProgressTimer.Elapsed += (e, b) => Progress();
            ProgressTimer.Start();
        }

        private void ResetWorld()
        {
            RequestScreenChange?.Invoke(new GameScreen());
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
            var facing = Player.GetFacingTile();

            if (facing == null) return;
            if (facing.WorldObject != null && facing.WorldObject.CanUse)
            {
                ContentChest.Sounds[facing.WorldObject.UseSound].Play();
                BeginProgress();
                return;
            }
            
            var slot = UIManager.GetSelectedSlot();
            if (slot.Item == null) return;
            if (!slot.Item.Usable) return;

            var protoType = ContentChest.ProtoTypes[slot.Item.FileName];
            var successful = protoType.CreateInstance(facing);
            if (!successful) return;

            ContentChest.Sounds[protoType.PlaceSound].Play();
            
            slot.Remove(1);
            AddWorldObject(facing.WorldObject);
        }

        private void BeginProgress()
        {
            RequestTransitionReset?.Invoke();
        }
        
        public void Progress()
        {
            foreach (var obj in WorldObjects)
            {
                obj.Progress();

                var drynessEffect = obj.GetDrynessEffect();
                var drynessRadius = obj.GetDrynessRadius();

                if (!obj.HasProgressEffect) continue;
                if (drynessRadius <= 0) continue;
                
                for (var i = obj.Tile.X - drynessRadius; i < obj.Tile.X + drynessRadius; i++)
                {
                    for (var j = obj.Tile.Y - drynessRadius; j < obj.Tile.Y + drynessRadius; j++)
                    {
                        var tile = Map.GetTileAt(i, j);
                        if (tile == null) continue;
                        tile.Dryness += drynessEffect;
                    }
                }
            }
        }

        private void DropItemNextTo(WorldObject worldObject)
        {
                var item = new DroppedItem()
                {
                    ItemName = worldObject.ObjectName[0],
                    FileName = worldObject.FileName[0],
                    Tile = Map.GetRandomTileInRadius(worldObject.Tile, 1),
                    Usable = true,
                };

                item.Tile.DroppedItem = item;

                if (Player.Tile == item.Tile)
                {
                    Player.PickupItemAt(item.Tile);
                }
        }

        private void AddWorldObject(WorldObject worldObject)
        {
            WorldObjects.Add(worldObject);
        }

        private void SetupPlayer()
        {
            var playerStartTile = Map.GetPlayerStartingTile();

            Player = new Player(playerStartTile, CreateStartingInventory())
            {
                OnTryMove = TryMove
            };

            UIManager.Inventory = Player.Inventory;
        }

        private Inventory CreateStartingInventory()
        {
            var inventory = new Inventory();

            foreach (var item in Map.GetStartingItems())
                inventory.AddItem(item, item.Count);
            
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