using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class KeyboardInput : IInput
    {

        private KeyboardState _lastState;
        
        public void Update(float delta)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.S))
            {
                if (_lastState.IsKeyUp(Keys.S))
                {
                    InputManager.OnDownPressed?.Invoke();
                }
                else
                {
                    InputManager.OnDownHeld?.Invoke();   
                }
            } else if (state.IsKeyDown(Keys.W)) 
            {
                if(_lastState.IsKeyUp(Keys.W)) {
                    InputManager.OnUpPressed?.Invoke();
                }
                else
                {
                    InputManager.OnUpHeld?.Invoke();
                }
                
            }

            if (state.IsKeyDown(Keys.D))
            {
                if (_lastState.IsKeyUp(Keys.D))
                {
                    InputManager.OnRightPressed?.Invoke();
                }
                else
                {
                    InputManager.OnRightHeld?.Invoke();   
                }
            } else if (state.IsKeyDown(Keys.A)) 
            {
                if(_lastState.IsKeyUp(Keys.A)) {
                    InputManager.OnLeftPressed?.Invoke();
                }
                else
                {
                    InputManager.OnLeftHeld?.Invoke();
                }
                
            }
            
            if (state.IsKeyDown(Keys.E) && _lastState.IsKeyUp(Keys.E))
            {
                InputManager.OnInteractPressed?.Invoke();
            }

            if (state.IsKeyDown(Keys.OemMinus) && _lastState.IsKeyUp(Keys.OemMinus))
            {
                InputManager.OnLastSlotPressed?.Invoke();
            } else if (state.IsKeyDown(Keys.OemPlus) && _lastState.IsKeyUp(Keys.OemPlus))
            {
                InputManager.OnNextSlotPressed?.Invoke();
            }

            if (state.IsKeyDown(Keys.Space) && _lastState.IsKeyUp(Keys.Space))
            {
                InputManager.OnPickupPressed?.Invoke();
            }

            if (state.IsKeyDown(Keys.Escape) && _lastState.IsKeyUp(Keys.Escape))
            {
                InputManager.OnBackPressed?.Invoke();
            }

            _lastState = state;
        }
    }
}