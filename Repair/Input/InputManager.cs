using System;
using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class InputManager
    {

        public Action OnGamePadConnected;
        private bool _notified;

        public void Update(float delta)
        {
            var state = GamePad.GetState(0);
            if (!state.IsConnected || _notified) return;
            
            _notified = true;
            OnGamePadConnected?.Invoke();
        }
        
    }
}