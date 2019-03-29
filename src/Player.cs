using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using Microsoft.Xna.Framework;

namespace flxkbr.unknownasofyet
{
    public class Player
    {
        public enum RSize { Small, Medium, Large }

        public ushort Health { get; set; } = 100;
        public ushort Mental { get; set; } = 100;
        public Point Position
        {
            get => _position;
            private set
            {
                _position = value;
                CurrentSprite.Position = value;
                collider.Location = value + currentColliderOffset;
            } 
        }
        public Entity CurrentSprite { get; private set; }

        public Rectangle Collider
        { 
            get => collider; 
            private set => this.collider = value;
        }
        public RSize RenderSize
        { 
            get => renderSize;
            set
            {
                switch(value)
                {
                    case RSize.Small:
                        this.CurrentSprite = smallSprite;
                        this.currentColliderOffset = new Point(0, 0);
                        this.collider = new Rectangle(this.Position + this.currentColliderOffset, new Point(1, 1));
                        break;
                    case RSize.Medium:
                        this.CurrentSprite = mediumSprite;
                        this.currentColliderOffset = new Point(0, 1);
                        this.collider = new Rectangle(this.Position + this.currentColliderOffset, new Point(1, 1));
                        break;
                    case RSize.Large:
                        this.CurrentSprite = largeSprite;
                        this.currentColliderOffset = new Point(0, 2);
                        this.collider = new Rectangle(this.Position + this.currentColliderOffset, new Point(2, 1));
                        break;
                }
                this.renderSize = value;
            }
        }

        private Point _position;
        private Entity smallSprite, mediumSprite, largeSprite;
        private Rectangle collider;
        private RSize renderSize;
        private Point currentColliderOffset;
        
        public Player()
        {
            initSprites();
            this.RenderSize = RSize.Medium;
        }

        public bool HandleInput(Keyboard info)
        {
            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.I))
            {
                System.Console.WriteLine($"RenderSize: {this.RenderSize}, Position: {this.Position}, Collider: {this.Collider}");
            }
            return false;
        }

        public void MoveTo(Point target)
        {
            this.Position = target;
        }

        public void MoveBy(Point delta)
        {
            this.Position = this.Position + delta;
        }

        private void initSprites()
        {
            smallSprite = new Entity(1, 1);
            mediumSprite = new Entity(1, 2);
            largeSprite = new Entity(2, 3);

            var smAnim = smallSprite.Animation.CurrentFrame[0];
            smAnim.Foreground = Globals.Colors.RedLight;
            smAnim.Background = Globals.Colors.BlueNight;
            smAnim.Glyph = '8';

            var medAnim = mediumSprite.Animation.CurrentFrame;
            medAnim[0].Foreground = medAnim[1].Foreground = Globals.Colors.RedLight;
            medAnim[0].Background = medAnim[1].Background = Globals.Colors.BlueNight;
            medAnim[0].Glyph = 'o';
            medAnim[1].Glyph = 'X';

            var lgAnim = largeSprite.Animation.CurrentFrame;
            foreach (var cell in lgAnim)
            {
                cell.Foreground = Globals.Colors.RedLight;
                cell.Background = Globals.Colors.BlueNight;
            }
            lgAnim[0].Glyph = '(';
            lgAnim[1].Glyph = ')';
            lgAnim[2].Glyph = lgAnim[3].Glyph = lgAnim[4].Glyph = lgAnim[5].Glyph = 179; // |
        }
    }
}