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

            if (state.ThumbSticks.Left.Y > 0.1f)
            {
                if (_lastState.ThumbSticks.Left.Y < 0.1f)
                {
                    InputManager.OnUpPressed?.Invoke();
                }
                else
                {
                    InputManager.OnUpHeld?.Invoke();
                }
            } else if (state.ThumbSticks.Left.Y < -0.1f)
            {
                if(_lastState.ThumbSticks.Left.Y > -0.1f)
                {
                    InputManager.OnDownPressed?.Invoke();
                }
                else
                {
                    InputManager.OnDownHeld?.Invoke();
                }
            }
            
            if (state.ThumbSticks.Left.X > 0.1f)
            {
                if (_lastState.ThumbSticks.Left.X < 0.1f)
                {
                    InputManager.OnRightPressed?.Invoke();
                }
                else
                {
                    InputManager.OnRightHeld?.Invoke();
                }
            } else if (state.ThumbSticks.Left.X < -0.1f)
            {
                if(_lastState.ThumbSticks.Left.X > -0.1f)
                {
                    InputManager.OnLeftPressed?.Invoke();
                }
                else
                {
                    InputManager.OnLeftHeld?.Invoke();
                }
            }

            if (state.Buttons.A == ButtonState.Pressed)
            {
                if (_lastState.Buttons.A == ButtonState.Released)
                {
                    InputManager.OnInteractPressed?.Invoke();
                }
            }

            if (state.Buttons.LeftShoulder == ButtonState.Pressed &&
                _lastState.Buttons.LeftShoulder == ButtonState.Released)
            {
                InputManager.OnZoomOutPressed?.Invoke();
            }

            if (state.Buttons.RightShoulder == ButtonState.Pressed &&
                _lastState.Buttons.RightShoulder == ButtonState.Released)
            {
                InputManager.OnZoomInPressed?.Invoke();
            }
            
            _lastState = state;
        }
    }
}