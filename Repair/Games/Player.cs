using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Games
{
    public class Player : IEntity
    {
        public Func<Tile, bool> OnTryMove;
        
        public Tile Tile { get; set; }
        public Tile TargetTile { get; set; }

        public Direction Facing = Direction.Down;

        private float Speed = 5f;
        public int PlayerSize { get; set; } = 24;
        public float MovementPercentage { get; set; }
        
        public Inventory Inventory { get; set; }

        public Tile GetFacingTile()
        {
            switch (Facing)
            {
                case Direction.Down:
                    return Tile.South;
                case Direction.Left:
                    return Tile.West;
                case Direction.Right:
                    return Tile.East;
                case Direction.Up:
                    return Tile.North;
                default:
                    return Tile;
            }
        }

        public Player(Tile startTile, Inventory startingInventory)
        {
            Tile = startTile;
            TargetTile = startTile;
            Inventory = startingInventory;
            Facing = Direction.Down;
        }

        public void Update(float delta)
        {
            if (Tile == TargetTile)
            {
                MovementPercentage = 1;
                return;
            }
            
            ContentChest.Player[Facing].Update(delta);
            
            MovementPercentage += Speed * delta;
            MovementPercentage = MathHelper.Clamp(MovementPercentage, 0, 1);

            if (MovementPercentage < 1) return;

            Tile = TargetTile;
            if (Tile.DroppedItem != null)
            {
                PickupItemAt(Tile);
            }
        }

        public Texture2D ActiveImage => ContentChest.Player[Facing].Current;

        public Texture2D GetImage()
        {
            return ActiveImage;
        }

        public void PickupItemAt(Tile tile)
        {
            var item = tile.DroppedItem;

            if (item == null) return;

            var added = Inventory.AddItem(new Item()
            {
                FileName = item.FileName,
                Count = 1,
                ItemName = item.ItemName,
                Usable = item.Usable,
                RepairID = item.RepairID
            });

            if (!added) return;

            tile.DroppedItem = null;
            ContentChest.PickUp.Play();
        }

        public void Move(int x, int y)
        {
            if (Tile != TargetTile) return;

            Tile moveTile = null;
            Direction dir = Direction.None;
            
            if (x < 0)
            {
                moveTile = Tile.West;
                dir = Direction.Left;
            }
            if (x > 0)
            {
                moveTile = Tile.East;
                dir = Direction.Right;
            }

            if (y < 0)
            {
                moveTile = Tile.North;
                dir = Direction.Up;
            }

            if (y > 0)
            {
                moveTile = Tile.South;
                dir = Direction.Down;
            }

            var shouldMove = OnTryMove.Invoke(moveTile);

            if (dir == Direction.None || moveTile == null) return;
            
            Facing = dir;

            if (!shouldMove) return;
            
            MovementPercentage = 0;
            TargetTile = moveTile;
        }

        public void Look(int x, int y)
        {
            if (x < 0)
            {
                Facing = Direction.Left;
            }
            
            if (x > 0)
            {
                Facing = Direction.Right;
            }

            if (y < 0)
            {
                Facing = Direction.Up;
            }

            if (y > 0)
            {
                Facing = Direction.Down;
            }
        }
    }
}