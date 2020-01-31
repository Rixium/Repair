using Microsoft.Xna.Framework.Graphics;

namespace Repair.Notify
{
    public interface INotification
    {
        void Update(float delta);
        void Draw(SpriteBatch spriteBatch);
    }
}