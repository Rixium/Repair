using Microsoft.Xna.Framework;

namespace Repair.Util
{
    public class Camera
    {
        public int X;
        public int Y;
        public float Scale = 2;
        public const int MaxZoom = 3;
        public int ScrollSpeed { get; } = 3;

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
            Scale = MathHelper.Clamp(Scale, 1, MaxZoom);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition) => 
            Vector2.Transform(screenPosition,
                Matrix.Invert(Get()));

        public Vector2 WorldToScreen(Vector2 worldPosition) => Vector2.Transform(worldPosition, Get());
        
    }
}