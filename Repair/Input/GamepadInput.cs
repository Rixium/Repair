using System;
using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class GamepadInput : IInput
    {

        private GamePadState _lastState;
        private float HoldThreshold = 0.1f; 
        
        public void Update(float delta)
        {
            var state = GamePad.GetState(0);

            if (state.ThumbSticks.Left.Y > 0.3f)
            {
                if (_lastState.ThumbSticks.Left.Y < 0.3f)
                {
                    InputManager.OnUpPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    HoldTimer += delta;

                    if (HoldTimer >= HoldThreshold)
                    {
                        InputManager.OnUpHeld?.Invoke();
                    }
                }
            } else if (state.ThumbSticks.Left.Y < -0.3f)
            {
                if(_lastState.ThumbSticks.Left.Y > -0.3f)
                {
                    InputManager.OnDownPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    HoldTimer += delta;

                    if (HoldTimer >= HoldThreshold)
                    {
                        InputManager.OnDownHeld?.Invoke();
                    }
                }
            }
            
            if (state.ThumbSticks.Left.X > 0.3f)
            {
                if (_lastState.ThumbSticks.Left.X < 0.3f)
                {
                    InputManager.OnRightPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    HoldTimer += delta;

                    if (HoldTimer >= HoldThreshold)
                    {
                        InputManager.OnRightHeld?.Invoke();
                    }
                }
            } else if (state.ThumbSticks.Left.X < -0.3f)
            {
                if(_lastState.ThumbSticks.Left.X > -0.3f)
                {
                    InputManager.OnLeftPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    HoldTimer += delta;

                    if (HoldTimer >= HoldThreshold)
                    {
                        InputManager.OnLeftHeld?.Invoke();
                    }
                }
            }

            if (state.Buttons.A == ButtonState.Pressed)
            {
                if (_lastState.Buttons.A == ButtonState.Released)
                {
                    InputManager.OnInteractPressed?.Invoke();
                    HoldTimer = 0;
                }
            }

            if (state.Buttons.LeftShoulder == ButtonState.Pressed &&
                _lastState.Buttons.LeftShoulder == ButtonState.Released)
            {
                InputManager.OnLastSlotPressed?.Invoke();
            }

            if (state.Buttons.RightShoulder == ButtonState.Pressed &&
                _lastState.Buttons.RightShoulder == ButtonState.Released)
            {
                InputManager.OnNextSlotPressed?.Invoke();
            }

            if (state.Buttons.X == ButtonState.Pressed)
            {
                if (_lastState.Buttons.X == ButtonState.Released)
                {
                    InputManager.OnPickupPressed?.Invoke();
                }
            }
            
            
            if (state.Buttons.Back == ButtonState.Pressed)
            {
                if (_lastState.Buttons.Back == ButtonState.Released)
                {
                    InputManager.OnBackPressed?.Invoke();
                }
            }

            _lastState = state;
        }

        public float HoldTimer { get; set; }
    }
}