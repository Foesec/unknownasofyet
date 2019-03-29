using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using System.IO;
using static flxkbr.unknownasofyet.InputMappings.InputAction;

namespace flxkbr.unknownasofyet
{
    public class WorldPanel : Console
    {
        readonly Color BorderColor = Globals.Colors.White;
        readonly Color BackgroundColor = Globals.Colors.Black;
        
        public Player Player { get; private set; }

        public GameMap CurrentGameMap;

        public WorldPanel() : base(Globals.WorldWidth, Globals.WorldHeight)
        {
            this.Position = new Point(1, 1);
            this.Player = new Player();
        }

        public bool HandleInput(Keyboard info)
        {
            Point movement = new Point(0, 0);
            switch (InputMappings.MovementPressed(info))
            {
                case Up:
                    movement.Y = -1;
                    break;
                case Down:
                    movement.Y = 1;
                    break;
                case Left:
                    movement.X = -1;
                    break;
                case Right:
                    movement.X = 1;
                    break;
                default:
                    break;
            }

            if (movement.X != 0 || movement.Y != 0)
            {
                var target = new Rectangle(
                        Player.Collider.X + movement.X,
                        Player.Collider.Y + movement.Y,
                        Player.Collider.Width,
                        Player.Collider.Height);
                if (CurrentGameMap.IsWalkable(target))
                {
                    Player.MoveBy(movement);
                }
            }

            return false;
        }

        public override void Update(System.TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
        }

        public void LoadLevel(string levelId)
        {
            this.Children.Clear();
            CurrentGameMap = GameMap.Get(levelId);
            this.Children.Add(CurrentGameMap.MapConsole);
            Player.RenderSize = Player.RSize.Large;
            Player.MoveTo(CurrentGameMap.MapData.Spawn);
            this.Children.Add(Player.CurrentSprite);
            WorldEntity ent = new WorldEntity("horsey", 4, 6, new Rectangle(0, 0, 3, 1), new Point(0, 5));
            ent.Repeat = true;
            ent.AnimationDuration = 1;
            ent.Start();
            this.Children.Add(ent);
        }
    }
}