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

        public static Action OnDownPressed { get; set; }
        public static Action OnUpPressed { get; set; }
        public static Action OnLeftPressed { get; set; }
        public static Action OnRightPressed { get; set; }
        public static Action OnInteractPressed { get; set; }
        public static Action OnDownHeld { get; set; }
        public static Action OnUpHeld { get; set; }
        public static Action OnRightHeld { get; set; }
        public static Action OnLeftHeld { get; set; }
        public static Action OnNextSlotPressed { get; set; }
        public static Action OnLastSlotPressed { get; set; }
        public static Action OnPickupPressed { get; set; }
        public static Action OnBackPressed { get; set; }


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

        public static IInput GetActiveInput() => _activeInput;
    }
}