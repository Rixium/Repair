using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Repair.Input;
using Repair.UI;

namespace Repair.Screen
{
    public class MainMenuScreen : IScreen
    {
        public Action RequestQuit { get; set; }
        public Action<IScreen> RequestScreenChange { get; set; }

        private const int ButtonPadding = 10;
        
        private int _activeButton;
        private Button[] _buttons;

        public MainMenuScreen()
        {
            //MediaPlayer.Play(ContentChest.MainMusic); // TODO UNCOMMENT ON RELEASE
            
            var startButton = new Button(ContentChest.ButtonFont, "Start", Color.Black, new Vector2(20, 100), Origin.Center);
            startButton.OnClick = OnStartClicked;
            
            var quitButton = new Button(ContentChest.ButtonFont, "Quit", Color.Black, new Vector2(20, startButton.Bottom + ButtonPadding), Origin.Center);
            quitButton.OnClick = OnQuitClicked;
            
            _buttons = new[]
            {
                startButton,
                quitButton
            };

            InputManager.OnDownPressed = OnDownPressed;
            InputManager.OnUpPressed = OnUpPressed;
            InputManager.OnInteractPressed = OnInteractPressed;
        }

        private void OnQuitClicked()
        {
            RequestQuit?.Invoke();
        }

        private void OnStartClicked()
        {
            RequestScreenChange?.Invoke(new GameScreen());
        }

        private void OnInteractPressed()
        {
            _buttons[_activeButton].Click();
        }

        private void OnUpPressed()
        {
            var newActive = _activeButton - 1;
            if (newActive < 0)
                newActive = _buttons.Length - 1;

            _activeButton = newActive;
        }

        private void OnDownPressed()
        {
            var newActive = _activeButton + 1;
            if (newActive >= _buttons.Length)
                newActive = 0;

            _activeButton = newActive;
        }

        public void Update(float delta)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(ContentChest.TitleFont, "Repair", new Vector2(20, 20), new Color(119, 221, 119));
            
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(ContentChest.ButtonFont, "<", _buttons[_activeButton].Right + new Vector2(10, 0), new Color(119, 221, 119));

            spriteBatch.End();
        }
        
    }
}