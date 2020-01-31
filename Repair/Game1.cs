﻿using System.Net.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Repair.Input;
using Repair.Notify;
using Repair.Screen;

namespace Repair
{
    public class Game1 : Game
    {
        private static string ApiKey { get; set; } = "b3102289f4ec9791eb056bc05cf7cbba";
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private WeatherManager _weatherManager;
        private ScreenManager _screenManager;
        private NotifyManager _notifyManager;

        public Game1()
        {
            InitializeScreenProperties();
            
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenProperties.ScreenWidth,
                PreferredBackBufferHeight = ScreenProperties.ScreenHeight
            };
            
            Content.RootDirectory = "Content";
        }

        private static void InitializeScreenProperties()
        {
            ScreenProperties.ScreenWidth = 1280;
            ScreenProperties.ScreenHeight = 720;
        }

        protected override void Initialize()
        {
            ContentChest.Initialize(Content);
            
            _weatherManager = new WeatherManager(new HttpClient(), ApiKey);
            var weatherInformation = _weatherManager.GetWeatherInformation();
            
            _notifyManager = new NotifyManager();
            _screenManager = new ScreenManager();

            InputManager.OnGamePadConnected += OnGamePadConnected;
            InputManager.OnGamePadDisconnected += OnGamePadDisconnected;
            base.Initialize();
        }

        private void OnGamePadDisconnected()
        {
            _notifyManager.OnGamePadDisconnected();
            
            InputManager.SetActiveInput(new KeyboardInput());
        }

        private void OnGamePadConnected()
        {
            _notifyManager.OnGamePadConnected();
            
            InputManager.SetActiveInput(new GamepadInput());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ContentChest.BasicLoad();
            RequestScreenChange(new SplashScreen());
        }

        private void RequestScreenChange(IScreen screen)
        {
            screen.RequestScreenChange += RequestScreenChange;
            _screenManager.SetScreen(screen);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var delta = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            _notifyManager.Update(delta);
            InputManager.Update(delta);
            _screenManager.Update(delta);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _screenManager.Draw(_spriteBatch);
            
            _spriteBatch.Begin();
            _notifyManager.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
