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
        public static SpriteFont TitleFont { get; set; }
        public static SpriteFont ButtonFont { get; set; }
        
        public static Texture2D NotifyLeft { get; set; }
        public static Texture2D NotifyRight { get; set; }
        public static Texture2D NotifyMid { get; set; }
        public static SoundEffect NotifySound { get; set; }
        public static SoundEffect ClickSound { get; set; }
        public static SoundEffect SelectSound { get; set; }
        
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
            
            TitleFont = _contentManager.Load<SpriteFont>("Fonts/title");
            MainMusic = _contentManager.Load<Song>("Music/main");
            Water = _contentManager.Load<Texture2D>("Images/water");

            WaterEdgeFrames = new Texture2D[2];
            WaterEdgeFrames[0] = _contentManager.Load<Texture2D>("Images/water_edge");
            WaterEdgeFrames[1] = _contentManager.Load<Texture2D>("Images/water_edge_2");
            
            ClickSound = _contentManager.Load<SoundEffect>("click");
            SelectSound = _contentManager.Load<SoundEffect>("select");

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
                Maps.Add(int.Parse(fileName), JsonConvert.DeserializeObject<MapData>(data));
            }
            
            SlotBorder = _contentManager.Load<Texture2D>("Images/slot_border");
            SlotBackground = _contentManager.Load<Texture2D>("Images/slot_background");
            SlotFont = _contentManager.Load<SpriteFont>("Fonts/slot");
            
            var text = File.ReadAllText($"{_contentManager.RootDirectory}/Raw/prototypes.json", Encoding.UTF8);
            var prototypes = JsonConvert.DeserializeObject<WorldObject[]>(text);
            
            foreach(var prototype in prototypes)
            {
                ProtoTypes.Add(prototype.FileName[0], prototype);
            }
            
            OnLoaded?.Invoke();
            
        }

    }
}