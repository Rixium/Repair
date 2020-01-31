using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public class KeyboardInput : IInput
    {

        private KeyboardState _lastState;
        
        public void Update(float delta)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.S) && _lastState.IsKeyUp(Keys.S))
            {
                InputManager.OnDownPressed();
            } else if (state.IsKeyDown(Keys.W) && _lastState.IsKeyUp(Keys.W))
            {
                InputManager.OnUpPressed();
            }

            _lastState = state;
        }
    }
}