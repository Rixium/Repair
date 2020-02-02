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

        public List<DroppedItem> WorldItems { get; set; }

        public UIManager UIManager;
        public int MapSize { get; set; } = 300;

        private Camera _camera;
        public Action<string> RequestNotification { get; set; }
        public Map Map { get; }
        private int _currentLevel = 1;
        public Player Player { get; set; }

        private Timer ProgressTimer;
        
        public World(int currentLevel)
        {
            _currentLevel = currentLevel;
            
            Map = new Map(ContentChest.Maps[_currentLevel]);

            WorldObjects = Map.WorldObjects;
            WorldItems = Map.WorldItems;
            
            InputManager.OnDownHeld = () => Player.Move(0, 1);
            InputManager.OnUpHeld = () => Player.Move(0, -1);
            InputManager.OnLeftHeld = () => Player.Move(-1, 0);
            InputManager.OnRightHeld = () => Player.Move(1, 0);
            InputManager.OnDownPressed = () => Player.Look(0, 1);
            InputManager.OnUpPressed = () => Player.Look(0, -1);
            InputManager.OnLeftPressed = () => Player.Look(-1, 0);
            InputManager.OnRightPressed = () => Player.Look(1, 0);
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
            RequestScreenChange?.Invoke(new GameScreen(_currentLevel));
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
            var successful = false;
            
            if (!slot.Item.Usable && slot.Item.RepairID != 0)
            {
                if (facing.WorldObject != null && facing.WorldObject.Repairable)
                {
                    successful = facing.WorldObject.Repair(slot.Item.ItemName);

                    if (successful)
                    {
                        ContentChest.InsertSound.Play();
                    }
                    if (facing.WorldObject.Repaired && facing.WorldObject.EndsLevelOnRepair)
                    {
                        var timer = new Timer
                        {
                            Interval = 2000
                        };

                        ContentChest.WindMillSound.Play();
                        timer.Elapsed += (e, b) =>
                        {
                            timer.Stop();
                            if (GameScreen.LevelExists(_currentLevel + 1))
                            {
                                RequestScreenChange?.Invoke(new GameScreen(_currentLevel + 1));
                            }
                            else
                            {
                                RequestScreenChange?.Invoke(new FinishScreen());
                            }
                        };

                        timer.Start();

                    }
                }
            }
            else
            {
                var protoType = ContentChest.ProtoTypes[slot.Item.FileName];
                successful = protoType.CreateInstance(facing);

                if (successful)
                {
                    ContentChest.Sounds[protoType.PlaceSound].Play();
                    PowerUpNeighbours(facing);
                    AddWorldObject(facing.WorldObject);
                }
            }
            
            if (!successful) return;

            slot.Remove(1);
        }

        private void PowerUpNeighbours(Tile tile)
        {
            if (tile?.WorldObject == null) return;
            
            var obj = tile.WorldObject;
            
            var left = tile.West;
            var right = tile.East;
            var top = tile.North;
            var bottom = tile.South;
            var accumulated = 0;
            var max = accumulated;

            if (left.WorldObject != null && left.WorldObject.ObjectType == obj.ObjectType)
            {
                left.WorldObject.AddPower();

                if (left.WorldObject.Power > max)
                    max = left.WorldObject.Power;
                
                accumulated++;
            }
            
            if (right.WorldObject != null && right.WorldObject.ObjectType == obj.ObjectType)
            {
                right.WorldObject.AddPower();
                
                if (right.WorldObject.Power > max)
                    max = right.WorldObject.Power;

                accumulated++;
            }
            
            if (top.WorldObject != null && top.WorldObject.ObjectType == obj.ObjectType)
            {
                top.WorldObject.AddPower();
                
                if (top.WorldObject.Power > max)
                    max = top.WorldObject.Power;
                
                accumulated++;
            }
            
            if (bottom.WorldObject != null && bottom.WorldObject.ObjectType == obj.ObjectType)
            {
                bottom.WorldObject.AddPower();
                
                if (bottom.WorldObject.Power > max)
                    max = bottom.WorldObject.Power;

                accumulated++;
            }

            if (accumulated > max)
                max = accumulated;

            obj.Power = accumulated;

            if (max >= 1)
            {
                ContentChest.Combos[max - 1].Play(0.5f, 0, 0);
            }
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

                var northEast = obj.Tile.NorthEast;
                var northWest = obj.Tile.NorthWest;
                var southEast = obj.Tile.SouthEast;
                var southWest = obj.Tile.SouthWest;

                if (northEast != null)
                    northEast.Dryness += drynessEffect;
                if (northWest != null)
                    northWest.Dryness += drynessEffect;
                if (southEast != null)
                    southEast.Dryness += drynessEffect;
                if (southWest != null)
                    southWest.Dryness += drynessEffect;

                var dried = false;
                for (var i = obj.Tile.X - drynessRadius; i < obj.Tile.X + drynessRadius; i++)
                {
                    var tile = Map.GetTileAt(i, obj.Tile.Y);
                    if (tile == null) continue;
                    var didDry = tile.AddDryness(drynessEffect);
                    if (didDry) dried = didDry;
                }
                
                for (var j = obj.Tile.Y - drynessRadius; j < obj.Tile.Y + drynessRadius; j++)
                {
                    var tile = Map.GetTileAt(obj.Tile.X, j);
                    if (tile == null) continue;
                    var didDry = tile.AddDryness(drynessEffect);
                    if (didDry) dried = didDry;
                }

                if (dried)
                {
                    ContentChest.PopSound.Play();
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
            WorldObjects.ForEach(m => m.Update(delta));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Get());

            var stillToDraw = Map.Draw(spriteBatch, _camera, Player);
            
            DrawPlayer(spriteBatch);

            while (stillToDraw.Count > 0)
            {
                Map.DrawObject(spriteBatch, stillToDraw.Dequeue());
            }
            
            spriteBatch.End();
        }

        private void DrawPlayer(SpriteBatch spriteBatch)
        {
            var drawVector = Player.Tile.WorldPosition;
            var targetVector = Player.TargetTile.WorldPosition;

            drawVector -= (drawVector - targetVector) * Player.MovementPercentage;
            drawVector += new Vector2(Map.TileSize / 2 - Player.ActiveImage.Width / 2, Map.TileSize / 2 - Player.ActiveImage.Height + 5);
            
            spriteBatch.Draw(Player.ActiveImage, new Rectangle((int) drawVector.X, (int) drawVector.Y, Player.ActiveImage.Width, Player.ActiveImage.Height), Color.White);
        }

    }

}