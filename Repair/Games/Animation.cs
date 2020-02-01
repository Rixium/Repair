using Microsoft.Xna.Framework.Graphics;

namespace Repair.Games
{
    public class Animation
    {

        private float _speed;
        private float _currentFrameTime;
        private int _currentFrame;
        private Texture2D[] _frames;

        public Texture2D Current => _frames[_currentFrame];

        public Animation(Texture2D[] frames, float speed)
        {
            _frames = frames;
            _speed = speed;
        }

        public void Update(float delta)
        {
            _currentFrameTime += delta;

            if (_currentFrameTime > _speed)
            {
                _currentFrame++;

                if (_currentFrame >= _frames.Length)
                {
                    _currentFrame = 0;
                }

                _currentFrameTime = 0;
            }
        }
        
    }
}