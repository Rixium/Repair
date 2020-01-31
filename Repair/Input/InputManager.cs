using System;
using Microsoft.Xna.Framework.Input;

namespace Repair.Input
{
    public static class InputManager
    {

        private static IInput _activeInput = new KeyboardInput();

        public static Action OnGamePadConnected;
        public static Action OnGamePadDisconnected;
        private static bool _wasConnected;

        public static Action OnDownPressed;
        public static Action OnUpPressed;
        public static Action OnLeftPressed;
        public static Action OnRightPressed;
        public static Action OnInteractPressed;

        public static void Update(float delta)
        {
            _activeInput.Update(delta);

            var state = GamePad.GetState(0);

            if (_wasConnected && !state.IsConnected)
            {
                OnGamePadDisconnected?.Invoke();
                _wasConnected = false;
                
                return;
            }

            if (!_wasConnected && state.IsConnected)
            {
                _wasConnected = true;
                OnGamePadConnected?.Invoke();
            }
        }

        public static void SetActiveInput(IInput input)
        {
            _activeInput = input;
        }
        
    }
}