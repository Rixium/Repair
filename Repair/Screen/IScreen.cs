using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Screen
{
    public interface IScreen
    {

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}