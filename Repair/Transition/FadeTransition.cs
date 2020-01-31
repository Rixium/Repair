using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Transition
{
    public class FadeTransition : ITransition
    {

        public bool Ended { get; set; }
        
        private bool _fadingOut;
        private bool _fadingIn;

        public Action OnTransitionInEnded { get; set; }
        public Action OnTransitionOutEnded { get; set; }

        private const float FadeSpeed = 0.5f;
        private float _fade = 1.0f;
        
        public bool HasEnded() => Ended;
        
        public void Reset()
        {
            _fadingOut = true;
            _fadingIn = false;
            
            Ended = false;
        }

        public void Update(float delta)
        {
            if (Ended) return;
            
            if (_fadingOut)
            {
                _fade += FadeSpeed * delta;

                if (_fade < 1.0f) return;
                
                _fade = 1;
                _fadingOut = false;
                _fadingIn = true;
                
                OnTransitionOutEnded?.Invoke();
            } else if (_fadingIn)
            {
                _fade -= FadeSpeed * delta;
                
                if (_fade > 0) return;
                
                _fade = 0;
                _fadingOut = false;
                _fadingIn = false;
                Ended = true;
                
                OnTransitionInEnded?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentChest.Pixel, new Rectangle(0, 0, ScreenProperties.ScreenWidth, ScreenProperties.ScreenHeight), Color.Black * _fade);
        }

    }
}