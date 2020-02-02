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
        public Action<string> RequestNotification { get; set; }
        public UIManager UIManager { get; set; }
        public Color BackColor { get; set; } = Color.White;
        public bool ShouldUpdateInputManager { get; set; } = true;

        private const int ButtonPadding = 10;
        
        private int _activeButton;
        private Button[] _buttons;

        public MainMenuScreen()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.Play(ContentChest.MainMusic);
            }

            var startText = "Start";
            var quitText = "Quit";
            
            var size = ContentChest.ButtonFont.MeasureString(startText);
            var middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - size.X / 2, ScreenProperties.ScreenHeight / 2.0f);
            
            var startButton = new Button(ContentChest.ButtonFont, startText, Color.Black, middle)
            {
                OnClick = OnStartClicked
            };

            
            size = ContentChest.ButtonFont.MeasureString(quitText);
            middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - size.X / 2, ScreenProperties.ScreenHeight / 2.0f + 40);

            var quitButton = new Button(ContentChest.ButtonFont, quitText, Color.Black, middle)
            {
                OnClick = OnQuitClicked
            };

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
            InputManager.OnDownPressed = null;
            InputManager.OnUpPressed = null;
            InputManager.OnInteractPressed = null;
            
            RequestScreenChange?.Invoke(new GameScreen());
        }

        private void OnInteractPressed()
        {
            ContentChest.SelectSound.Play();
            _buttons[_activeButton].Click();
        }

        private void OnUpPressed()
        {
            ContentChest.ClickSound.Play();
            
            var newActive = _activeButton - 1;
            if (newActive < 0)
                newActive = _buttons.Length - 1;

            _activeButton = newActive;
        }

        private void OnDownPressed()
        {
            ContentChest.ClickSound.Play();
            
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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            var titleSize = ContentChest.TitleFont.MeasureString(GameProperties.Title);
            var middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - titleSize.X / 2, ScreenProperties.ScreenHeight / 2.0f - titleSize.X - 20);
            spriteBatch.DrawString(ContentChest.TitleFont, GameProperties.Title, middle, new Color(119, 221, 119));
            
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(ContentChest.ButtonFont, "<", _buttons[_activeButton].Right + new Vector2(10, 0), new Color(119, 221, 119));

            spriteBatch.End();
        }
        
    }
}