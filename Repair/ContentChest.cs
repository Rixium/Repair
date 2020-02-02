using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Repair.Games;

namespace Repair
{
    public static class ContentChest
    {

        private static bool _loadInitialized;
        public static Action OnLoaded;
        
        private static ContentManager _contentManager;

        public static Texture2D Pixel { get; set; }
        public static Texture2D Splash { get; set; }
        public static Song MainMusic { get; set; }
        public static Song GameMusic { get; set; }
        public static SpriteFont TitleFont { get; set; }
        public static SpriteFont ButtonFont { get; set; }
        public static SpriteFont CreditFont { get; set; }
        
        public static Texture2D NotifyLeft { get; set; }
        public static Texture2D NotifyRight { get; set; }
        public static Texture2D NotifyMid { get; set; }
        public static SoundEffect NotifySound { get; set; }
        public static SoundEffect ClickSound { get; set; }
        public static SoundEffect SelectSound { get; set; }
        public static SoundEffect PopSound { get; set; }
        public static SoundEffect InsertSound { get; set; }
        
        public static Texture2D Grass { get; set; }
        public static Texture2D Water { get; set; }
        public static Texture2D[] WaterEdgeFrames { get; set; }
        
        public static Dictionary<string, Texture2D> Items { get; set; }
        public static Dictionary<string, Texture2D> WorldObjects { get; set; }
        public static Dictionary<string, WorldObject> ProtoTypes { get; set; }
        public static Dictionary<int, MapData> Maps { get; set; }
        public static Texture2D SlotBorder { get; set; }
        public static Texture2D SlotBackground { get; set; }
        public static SpriteFont SlotFont { get; set; }
        public static Dictionary<string, SoundEffect> Sounds { get; set; }
        public static Dictionary<Direction, Animation> Player { get; set; }
        public static SoundEffect PickUp { get; set; }
        public static SoundEffect WindMillSound { get; set; }
        public static SoundEffect[] Combos { get; set; }
        public static SoundEffect[] Walk { get; set; }
        public static SoundEffect SplashSound { get; set; }

        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static void BasicLoad()
        {            
            ButtonFont = _contentManager.Load<SpriteFont>("Fonts/button");
            NotifyLeft = _contentManager.Load<Texture2D>("Images/notify_left");
            NotifyRight = _contentManager.Load<Texture2D>("Images/notify_right");
            NotifyMid = _contentManager.Load<Texture2D>("Images/notify_mid");
            Pixel = _contentManager.Load<Texture2D>("Images/pixel");
            Splash = _contentManager.Load<Texture2D>("Images/splash");
            NotifySound = _contentManager.Load<SoundEffect>("notify");
            SplashSound = _contentManager.Load<SoundEffect>("splash");
        }


        public static void Load()
        {
            if (_loadInitialized) return;
            
            _loadInitialized = true;
            
            Items = new Dictionary<string, Texture2D>();
            WorldObjects = new Dictionary<string, Texture2D>();
            ProtoTypes = new Dictionary<string, WorldObject>();
            Sounds = new Dictionary<string, SoundEffect>();
            Maps = new Dictionary<int, MapData>();
            Combos = new SoundEffect[4];
            
            TitleFont = _contentManager.Load<SpriteFont>("Fonts/title");
            MainMusic = _contentManager.Load<Song>("Music/main");
            GameMusic = _contentManager.Load<Song>("Music/game");
            Water = _contentManager.Load<Texture2D>("Images/water");

            WaterEdgeFrames = new Texture2D[2];
            WaterEdgeFrames[0] = _contentManager.Load<Texture2D>("Images/water_edge");
            WaterEdgeFrames[1] = _contentManager.Load<Texture2D>("Images/water_edge_2");
            
            Player = new Dictionary<Direction, Animation>();
            
            ClickSound = _contentManager.Load<SoundEffect>("click");
            SelectSound = _contentManager.Load<SoundEffect>("select");
            PickUp = _contentManager.Load<SoundEffect>("pickup");
            WindMillSound = _contentManager.Load<SoundEffect>("windmill");

            Combos[0] = _contentManager.Load<SoundEffect>("combo1");
            Combos[1]   = _contentManager.Load<SoundEffect>("combo2");
            Combos[2]  = _contentManager.Load<SoundEffect>("combo3");
            Combos[3]  = _contentManager.Load<SoundEffect>("combo4");
            

            Walk = new[]
            {
                _contentManager.Load<SoundEffect>("walk1"),
                _contentManager.Load<SoundEffect>("walk2"),
                _contentManager.Load<SoundEffect>("walk3"),
                _contentManager.Load<SoundEffect>("walk4")
            };
            
            Grass = _contentManager.Load<Texture2D>("Images/grass");
            
            foreach (var file in Directory.GetFiles(_contentManager.RootDirectory + "/Images/Items"))
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                Items.Add(fileName, _contentManager.Load<Texture2D>($"Images/Items/{fileName}"));
            }
            
            foreach (var file in Directory.GetFiles(_contentManager.RootDirectory + "/Images/WorldObjects"))
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                WorldObjects.Add(fileName, _contentManager.Load<Texture2D>($"Images/WorldObjects/{fileName}"));
            }
            
            foreach (var file in Directory.GetFiles(_contentManager.RootDirectory + "/Sounds"))
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                Sounds.Add(fileName, _contentManager.Load<SoundEffect>($"Sounds/{fileName}"));
            }

            foreach (var file in Directory.GetFiles(_contentManager.RootDirectory + "/Maps"))
            {
                var fileName = Path.GetFileName(file).Split('.')[0];
                var data = File.ReadAllText($"{_contentManager.RootDirectory}/Maps/{fileName}.json", Encoding.UTF8);
                var map = JsonConvert.DeserializeObject<MapData>(data);
                map.Data = map.Layers[0].Data;
                
                Maps.Add(int.Parse(fileName), map);
            }
            
            SlotBorder = _contentManager.Load<Texture2D>("Images/slot_border");
            SlotBackground = _contentManager.Load<Texture2D>("Images/slot_background");
            SlotFont = _contentManager.Load<SpriteFont>("Fonts/slot");
            CreditFont = _contentManager.Load<SpriteFont>("Fonts/credits");

            PopSound = _contentManager.Load<SoundEffect>("pop");
            InsertSound = _contentManager.Load<SoundEffect>("insert");

            var text = File.ReadAllText($"{_contentManager.RootDirectory}/Raw/prototypes.json", Encoding.UTF8);
            var prototypes = JsonConvert.DeserializeObject<WorldObject[]>(text);
            
            foreach(var prototype in prototypes)
            {
                ProtoTypes.Add(prototype.FileName[0], prototype);
            }
            
            var downFrames = new Texture2D[]
            {
                _contentManager.Load<Texture2D>("down1"),
                _contentManager.Load<Texture2D>("down2")
            };
            var downAnimation = new Animation(downFrames, 0.1f);
            
            var upFrames = new Texture2D[]
            {
                _contentManager.Load<Texture2D>("up1"),
                _contentManager.Load<Texture2D>("up2")
            };
            var upAnimation = new Animation(upFrames, 0.1f);
            
            var leftFrames = new Texture2D[]
            {
                _contentManager.Load<Texture2D>("left1"),
                _contentManager.Load<Texture2D>("left2")
            };
            var leftAnimation = new Animation(leftFrames, 0.1f);
            
            var rightFrames = new Texture2D[]
            {
                _contentManager.Load<Texture2D>("right1"),
                _contentManager.Load<Texture2D>("right2")
            };
            var rightAnimation = new Animation(rightFrames, 0.1f);
            
            Player.Add(Direction.Down, downAnimation);
            Player.Add(Direction.Up, upAnimation);
            Player.Add(Direction.Left, leftAnimation);
            Player.Add(Direction.Right, rightAnimation);
            
            OnLoaded?.Invoke();
            
        }

    }
}