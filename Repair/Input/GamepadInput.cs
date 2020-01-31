using System;
using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class GamepadInput : IInput
    {

        private GamePadState _lastState;
        
        public void Update(float delta)
        {
            var state = GamePad.GetState(0);

            if (state.ThumbSticks.Left.Y > 0.1f && _lastState.ThumbSticks.Left.Y < 0.1f)
            {
                InputManager.OnUpPressed?.Invoke();
            } else if (state.ThumbSticks.Left.Y < -0.1f && _lastState.ThumbSticks.Left.Y > -0.1f)
            {
                InputManager.OnDownPressed?.Invoke();
            }

            _lastState = state;
        }
    }
}