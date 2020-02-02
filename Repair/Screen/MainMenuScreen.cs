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
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(ContentChest.MainMusic);
            
            var startText = "Start";
            var quitText = "Quit";
            
            var size = ContentChest.ButtonFont.MeasureString(startText);
            var middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - size.X / 2, ScreenProperties.ScreenHeight / 2.0f - 35);
            
            var startButton = new Button(ContentChest.ButtonFont, startText, Color.Black, middle)
            {
                OnClick = OnStartClicked
            };

            
            size = ContentChest.ButtonFont.MeasureString(quitText);
            middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - size.X / 2, ScreenProperties.ScreenHeight / 2.0f);

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
            
            MediaPlayer.Stop();
            MediaPlayer.Play(ContentChest.GameMusic);
#if DEBUG
            RequestScreenChange?.Invoke(new GameScreen(ContentChest.Maps.Count));
#else
            RequestScreenChange?.Invoke(new GameScreen());
#endif
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
            spriteBatch.Draw(ContentChest.MenuBackground, new Rectangle(0, 0, ScreenProperties.ScreenWidth, ScreenProperties.ScreenHeight), Color.White * 0.4f);
            var titleSize = ContentChest.TitleFont.MeasureString(GameProperties.Title);
            var middle = new Vector2(ScreenProperties.ScreenWidth / 2.0f - titleSize.X / 2, ScreenProperties.ScreenHeight / 2.0f - titleSize.X - 20);
            
            spriteBatch.Draw(ContentChest.MenuSelectBack, new Rectangle(ScreenProperties.ScreenWidth / 2 - ContentChest.MenuSelectBack.Width, 
                    ScreenProperties.ScreenHeight / 2 - ContentChest.MenuSelectBack.Height, ContentChest.MenuSelectBack.Width * 2, ContentChest.MenuSelectBack.Height * 2), Color.White);
            spriteBatch.DrawString(ContentChest.TitleFont, GameProperties.Title, middle, Color.White);
            
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(ContentChest.ButtonFont, "<", _buttons[_activeButton].Right + new Vector2(10, 0), new Color(119, 221, 119));

            var activeInput = InputManager.GetActiveInput();
            var pos = new Vector2(10, ScreenProperties.ScreenHeight - 200);
            var text = "Use: ";
            var textSize = ContentChest.ButtonFont.MeasureString(text);
            ContentChest.InputImages.TryGetValue($"{activeInput.Name}_Use".ToLower(), out var useTexture);
            var text1 = "Move: ";
            var textSize1 = ContentChest.ButtonFont.MeasureString(text1);
            var pos1 = new Vector2(10, pos.Y + 50);
            ContentChest.InputImages.TryGetValue($"{activeInput.Name}_Move".ToLower(), out var moveTexture);
            var pos2 = new Vector2(10, pos1.Y + 50);
            var text2 = "Inventory: ";
            var textSize2 = ContentChest.ButtonFont.MeasureString(text2);
            ContentChest.InputImages.TryGetValue($"{activeInput.Name}_Inventory".ToLower(), out var inventoryTexture);
            var pos3 = new Vector2(10, pos2.Y + 50);
            var text3 = "Reset: ";
            var textSize3 = ContentChest.ButtonFont.MeasureString(text3);
            ContentChest.InputImages.TryGetValue($"{activeInput.Name}_ResetLevel".ToLower(), out var resetLevelTexture);

            pos1.X = pos2.X + textSize2.X  - textSize1.X;
            pos3.X = pos2.X + textSize2.X - textSize3.X;
            pos.X = pos2.X  + textSize2.X - textSize.X;

            UIManager.DrawText(spriteBatch, ContentChest.ButtonFont, text, Color.Black, Color.White, 1, pos);
            spriteBatch.Draw(useTexture, new Vector2(pos.X + textSize.X + 10, pos.Y + textSize.Y / 2 - useTexture.Height / 2), Color.White);
            
            UIManager.DrawText(spriteBatch, ContentChest.ButtonFont, text1, Color.Black, Color.White, 1, pos1);
            spriteBatch.Draw(moveTexture, new Vector2(pos1.X  + textSize1.X + 10, pos1.Y + textSize1.Y / 2 - moveTexture.Height / 2), Color.White);
            
            UIManager.DrawText(spriteBatch, ContentChest.ButtonFont, text2, Color.Black, Color.White, 1, pos2);
            spriteBatch.Draw(inventoryTexture, new Vector2(pos2.X + textSize2.X + 10, pos2.Y + textSize2.Y / 2 - inventoryTexture.Height / 2), Color.White);
            
            UIManager.DrawText(spriteBatch, ContentChest.ButtonFont, text3, Color.Black, Color.White, 1, pos3);
            spriteBatch.Draw(resetLevelTexture, new Vector2(pos3.X + textSize3.X + 10, pos3.Y + textSize3.Y / 2 - resetLevelTexture.Height / 2), Color.White);
                
            spriteBatch.End();
        }
        
    }
}