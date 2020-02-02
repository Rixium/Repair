using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class KeyboardInput : IInput
    {

        private KeyboardState _lastState;
        private float HoldThreshold = 0.1f;
        public float HoldTimer { get; set; }

        public string Name { get; set; } = "Keyboard";

        public void Update(float delta)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.S))
            {
                if (_lastState.IsKeyUp(Keys.S))
                {
                    InputManager.OnDownPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    if (HoldTimer > HoldThreshold)
                    {
                        InputManager.OnDownHeld?.Invoke();
                    }
                    else
                    {
                        HoldTimer += delta;
                    }
                }
            } else if (state.IsKeyDown(Keys.W)) 
            {
                if(_lastState.IsKeyUp(Keys.W)) {
                    InputManager.OnUpPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    if (HoldTimer > HoldThreshold)
                    {
                        InputManager.OnUpHeld?.Invoke();
                    }
                    else
                    {
                        HoldTimer += delta;
                    }
                }
                
            }

            if (state.IsKeyDown(Keys.D))
            {
                if (_lastState.IsKeyUp(Keys.D))
                {
                    InputManager.OnRightPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    if (HoldTimer > HoldThreshold)
                    {
                        InputManager.OnRightHeld?.Invoke();   
                    }
                    else
                    {
                        HoldTimer += delta;
                    }
                }
            } else if (state.IsKeyDown(Keys.A)) 
            {
                if(_lastState.IsKeyUp(Keys.A)) {
                    InputManager.OnLeftPressed?.Invoke();
                    HoldTimer = 0;
                }
                else
                {
                    if (HoldTimer > HoldThreshold)
                    {
                        InputManager.OnLeftHeld?.Invoke();
                    }
                    else
                    {
                        HoldTimer += delta;
                    }
                }
                
            }
            
            if (state.IsKeyDown(Keys.Space) && _lastState.IsKeyUp(Keys.Space))
            {
                InputManager.OnInteractPressed?.Invoke();
                HoldTimer = 0;
            }

            if (state.IsKeyDown(Keys.Tab) && _lastState.IsKeyUp(Keys.Tab))
            {
                InputManager.OnNextSlotPressed?.Invoke();
            }

            if (state.IsKeyDown(Keys.R) && _lastState.IsKeyUp(Keys.R))
            {
                InputManager.OnBackPressed?.Invoke();
            }

            _lastState = state;
        }
    }
}