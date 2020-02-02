using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Repair.Games;

namespace Repair.Util
{
    public class Camera
    {
        public float X;
        public float Y;
        public float Scale = 2;
        public const int MaxZoom = 2;
        public const int MinZoom = 2;
        public float ScrollSpeed { get; } = 0.5f;

        public IEntity FollowTarget { get; set; }
        
        public static Vector2 ViewportCenter => new Vector2(ScreenProperties.ScreenWidth * 0.5f, ScreenProperties.ScreenHeight * 0.5f);

        public Camera(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }

        public Matrix Get() =>
                Matrix.CreateTranslation(-X, -Y, 0) *
                Matrix.CreateScale(Scale, Scale, 1) *
                Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));

        public void Zoom(float delta)
        {
            Scale += delta;
            Scale = MathHelper.Clamp(Scale, MinZoom, MaxZoom);
        }

        public void Update(float delta)
        {
            if (FollowTarget == null) return;
            
            var positionVector = FollowTarget.Tile.WorldPosition;
            var targetVector = FollowTarget.TargetTile.WorldPosition;
            
            positionVector -= (positionVector - targetVector) * FollowTarget.MovementPercentage;

            if (positionVector.X < X)
            {
                X -=  (int) Math.Abs(positionVector.X - X) / 10;
            }

            if (positionVector.X > X)
            {
                X += (int)  Math.Abs(positionVector.X - X) / 10;
            }

            if (positionVector.Y < Y)
            {
                Y -= (int)  Math.Abs(positionVector.Y - Y) / 10;
            }
            if (positionVector.Y > Y)
            {
                Y += (int) Math.Abs(positionVector.Y - Y) / 10;
            }
        }
        
        public Vector2 ScreenToWorld(Vector2 screenPosition) => 
            Vector2.Transform(screenPosition,
                Matrix.Invert(Get()));

        public Vector2 WorldToScreen(Vector2 worldPosition) => Vector2.Transform(worldPosition, Get());

        public void SetFollowTarget(IEntity entity)
        {
            FollowTarget = entity;
        }

    }
}