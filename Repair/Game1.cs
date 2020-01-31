using System.Net.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Repair
{
    public class Game1 : Game
    {
        private static string ApiKey { get; set; } = "b3102289f4ec9791eb056bc05cf7cbba";
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private WeatherManager _weatherManager;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ContentChest.Initialize(Content);
            
            _weatherManager = new WeatherManager(new HttpClient(), ApiKey);
            var weatherInformation = _weatherManager.GetWeatherInformation();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ContentChest.Load();
            
            MediaPlayer.Play(ContentChest.MainMusic);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
