using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Repair.UI;

namespace Repair.Screen
{
    public interface IScreen
    {
        Action RequestQuit { get; set; }
        Action<IScreen> RequestScreenChange { get; set; }
        Action<string> RequestNotification { get; set; }
        UIManager UIManager { get; set; }

        void Update(float delta);

        void Draw(SpriteBatch spriteBatch);
    }
}