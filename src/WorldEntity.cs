using System.IO;
using SadConsole;
using SadConsole.Readers;
using Microsoft.Xna.Framework;

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

        private bool animate;
        private Rectangle collider;
        private Point colliderOffset;

        public WorldEntity(string name, int width, int height, Rectangle collider, Point colliderOffset) : base(name, width, height)
        {
            this.collider = collider;
            this.colliderOffset = colliderOffset;
            this.collider.X = colliderOffset.X;
            this.collider.Y = colliderOffset.Y;
            loadAnimation();
        }

        private void loadAnimation()
        {
            using (Stream inStream = File.OpenRead($"{Globals.XpPath}Sprites/{this.Name}.xp"))
            {
                LayeredConsole animLayers = REXPaintImage.Load(inStream).ToLayeredConsole();
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
            }
        }

        public void MoveTo(int x, int y)
        {
            this.Position = new Point(x, y);
            this.collider.Location = this.Position + colliderOffset;
        }
    }
}