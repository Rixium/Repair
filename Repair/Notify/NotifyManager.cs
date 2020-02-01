using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Repair.Notify
{
    public class NotifyManager
    {

        private List<INotification> _notifications = new List<INotification>();
        
        public void Update(float delta)
        {
            _notifications.ForEach(n => n.Update(delta));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _notifications.ForEach(n => n.Draw(spriteBatch));
        }

        public void OnGamePadConnected()
        {
            ContentChest.NotifySound.Play();
            _notifications.Add(new Notification("Gamepad Connected"));
        }


        public void OnGamePadDisconnected()
        {
            ContentChest.NotifySound.Play();
            _notifications.Add(new Notification("Gamepad Disconnected"));
        }

        public void OnRequestNotification(string text)
        {
            ContentChest.NotifySound.Play();
            _notifications.Add(new Notification(text));
        }
    }
}