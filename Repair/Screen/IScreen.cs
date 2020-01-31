using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Screen
{
    public interface IScreen
    {
        Action RequestQuit { get; set; }
        Action<IScreen> RequestScreenChange { get; set; }
        
        void Update(float delta);

        void Draw(SpriteBatch spriteBatch);
    }
}