using Microsoft.Xna.Framework.Graphics;

namespace Repair.Games
{
    public class Animation
    {

        public float Speed { get; set; }
        private float _currentFrameTime;
        private int _currentFrame;
        public Texture2D[] Frames { get; set; }
        public string[] FrameNames { get; set; }

        public Texture2D Current =>
            Frames == null ? 
                ContentChest.WorldObjects[FrameNames[_currentFrame]] : 
                Frames[_currentFrame];

        public Animation(Texture2D[] frames, float speed)
        {
            Frames = frames;
            Speed = speed;
        }

        public void Update(float delta)
        {
            _currentFrameTime += delta;

            if (_currentFrameTime > Speed)
            {
                _currentFrame++;

                if (_currentFrame >= FramesCount)
                {
                    _currentFrame = 0;
                }

                _currentFrameTime = 0;
            }
        }

        public int FramesCount => Frames?.Length ?? FrameNames.Length;
        
    }
}