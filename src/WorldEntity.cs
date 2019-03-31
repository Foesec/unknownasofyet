using System.IO;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Readers;
using Microsoft.Xna.Framework;
using Serilog;
using flxkbr.unknownasofyet.state;

namespace flxkbr.unknownasofyet
{
    public class WorldEntity : AnimatedConsole
    {
        
        public Rectangle Collider
        {
            get => collider;
        }

        public bool Animate
        {
            get => animate;
            set
            {
                if (!animate && value)
                {
                    this.animate = value;
                    this.Start();
                }
                else if (animate && !value)
                {
                    this.animate = value;
                    this.Stop();
                }
            }
        }

        public EntityData Data { get; set; }

        private bool animate;
        private Rectangle collider;
        private Point colliderOffset;

        private static Dictionary<string, LayeredConsole> spriteCache;

        public static WorldEntity GetInstance(string name)
        {
            EntityData data = EntityDataStorage.Get(name);
            return new WorldEntity(name, data);
        }

        public static void LoadSprites()
        {
            if (spriteCache != null) return;

            spriteCache = new Dictionary<string, LayeredConsole>();
            foreach (var entityName in Globals.SpriteList)
            {
                using (Stream inStream = File.OpenRead($"{Globals.XpPath}Sprites/{entityName}.xp"))
                {
                    LayeredConsole sprite = REXPaintImage.Load(inStream).ToLayeredConsole();
                    spriteCache.Add(entityName, sprite);
                }
            }
            Log.Logger.Information("Loaded {Count} Sprites", spriteCache.Count);
        }

        private WorldEntity(string name, EntityData data) : base(name, data.Dimensions.X, data.Dimensions.Y)
        {
            this.Data = data;
            if (data.Collider != null)
            {
                this.colliderOffset = new Point(data.Collider.X, data.Collider.Y);
                this.collider = data.Collider;
            }
            else
            {
                this.colliderOffset = new Point();
                this.collider = new Rectangle(0, 0, 0, 0);
            }
            loadData();
        }

        private void loadData()
        {
            LayeredConsole animLayers = spriteCache[this.Name];
            for (var i = 0; i < animLayers.LayerCount; ++i)
            {
                CellSurfaceLayer layer = animLayers.GetLayer(i);
                CellSurface frame = this.CreateFrame();
                for (var y = 0; y < frame.Height; ++y)
                {
                    for (var x = 0; x < frame.Width; ++x)
                    {
                        Cell c = layer[x, y];
                        frame.SetBackground(x, y, c.Background);
                        frame.SetForeground(x, y, c.Foreground);
                        frame.SetGlyph(x, y, c.Glyph);
                    }
                }
            }
            if (this.Data.Bounce == true)
            {
                // go through layers in reverse, skipping the last and first frame
                for (var i = animLayers.LayerCount - 2; i > 0; --i)
                {
                    CellSurfaceLayer layer = animLayers.GetLayer(i);
                    CellSurface frame = this.CreateFrame();
                    for (var y = 0; y < frame.Height; ++y)
                    {
                        for (var x = 0; x < frame.Width; ++x)
                        {
                            Cell c = layer[x, y];
                            frame.SetBackground(x, y, c.Background);
                            frame.SetForeground(x, y, c.Foreground);
                            frame.SetGlyph(x, y, c.Glyph);
                        }
                    }
                }
            }
            this.AnimationDuration = this.Data.AnimDuration;
            this.Repeat = this.Data.Repeat;
        }

        public void MoveTo(int x, int y)
        {
            this.Position = new Point(x, y);
            this.collider.Location = this.Position + colliderOffset;
        }

        public void MoveTo(Point destination)
        {
            this.Position = destination;
            this.collider.Location = destination + colliderOffset;
        }
    }
}