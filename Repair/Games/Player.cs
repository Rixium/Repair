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

        private float Speed = 5f;
        public int PlayerSize { get; set; } = 24;
        public float MovementPercentage { get; set; }
        
        public Inventory Inventory { get; set; }

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
        }
        
        public void Move(int x, int y)
        {
            if (Tile != TargetTile) return;
            
            var moveTile = Tile.West;

            if (x > 0) moveTile = Tile.East;
            if (y < 0) moveTile = Tile.North;
            if (y > 0) moveTile = Tile.South;

            var shouldMove = OnTryMove.Invoke(moveTile);

            if (shouldMove)
            {
                MovementPercentage = 0;
                TargetTile = moveTile;
            }

        }

    }
}