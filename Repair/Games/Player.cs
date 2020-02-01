using Microsoft.Xna.Framework;

namespace Repair.Games
{
    public class Player : IEntity
    {

        public Vector2 Position { get; set; }
        public float Speed = 5f;

        public Player(Vector2 startVector)
        {
            Position = startVector;
        }

        public void Move(float x, float y)
        {
            var currentPosition = Position;
            currentPosition.X += x;
            currentPosition.Y += y;
            Position = currentPosition;
        }

    }
}