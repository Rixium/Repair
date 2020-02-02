using System;
using System.IO;
using Microsoft.Xna.Framework;

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
        }

        public void Update(float delta)
        {
            if (Tile == TargetTile)
            {
                MovementPercentage = 1;
                return;
            }
            
            MovementPercentage += Speed * delta;
            MovementPercentage = MathHelper.Clamp(MovementPercentage, 0, 1);

            if (MovementPercentage < 1) return;

            Tile = TargetTile;
            if (Tile.DroppedItem != null)
            {
                PickupItemAt(Tile);
            }
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
                Usable = item.Usable
            });

            if (!added) return;

            tile.DroppedItem = null;
            ContentChest.PickUp.Play();
        }

        public void Move(int x, int y)
        {
            if (Tile != TargetTile) return;
            
            var moveTile = Tile.West;
            var dir = Direction.Left;

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

            Facing = dir;

            if (!shouldMove) return;
            
            MovementPercentage = 0;
            TargetTile = moveTile;
        }

    }
}