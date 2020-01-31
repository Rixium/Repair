using System;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Transition
{
    public interface ITransition
    {

        Action OnTransitionInEnded { get; set; }
        Action OnTransitionOutEnded { get; set; }

        void Reset();
        
        void Update(float delta);

        void Draw(SpriteBatch spriteBatch);

        bool HasEnded();
    }
}