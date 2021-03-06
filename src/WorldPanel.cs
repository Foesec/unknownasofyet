using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using Serilog;
using flxkbr.unknownasofyet.state;
using static flxkbr.unknownasofyet.InputMappings.InputAction;

namespace flxkbr.unknownasofyet
{
    public class WorldPanel : SadConsole.Console
    {
        readonly Color BorderColor = Globals.Colors.White;
        readonly Color BackgroundColor = Globals.Colors.Black;
        
        public Player Player { get; private set; }
        public event EventHandler<Effect> EffectTriggered;

        public GameMap CurrentGameMap;

        private bool playerCanMove = true;

        public WorldPanel() : base(Globals.WorldWidth, Globals.WorldHeight)
        {
            this.Position = new Point(1, 1);
            this.Player = new Player();
        }

        public bool HandleInput(Keyboard info)
        {
            if (playerCanMove)
            {
                bool moved = handlePlayerMovement(info);
                if (moved)
                {
                    EventObject evnt = CurrentGameMap.GetEvent(this.Player.Collider);
                    if (evnt != null && evnt.Condition.Satisfied)
                    {
                        triggerEvent(evnt);
                    }
                }
            }
            handleInteraction(info);
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
            foreach (var entity in CurrentGameMap.MapData.Entities)
            {
                foreach (var location in entity.Locations)
                {
                    WorldEntity we = WorldEntity.GetInstance(entity.Entity);
                    we.MoveTo(location);
                    this.Children.Add(we);
                    we.Animate = true;
                }
            }
        }

        private bool handlePlayerMovement(Keyboard info)
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
                    return false;
            }
            var target = new Rectangle(
                    Player.Collider.X + movement.X,
                    Player.Collider.Y + movement.Y,
                    Player.Collider.Width,
                    Player.Collider.Height);
            if (CurrentGameMap.IsWalkable(target))
            {
                Player.MoveBy(movement);
                return true;
            }
            return false;
        }

        private bool handleInteraction(Keyboard info)
        {
            if (InputMappings.IsInteractionPressed(info))
            {
                InteractionObject interaction = CurrentGameMap.GetInteraction(this.Player.Collider);
                if (interaction != null && interaction.Condition.Satisfied)
                {
                    triggerInteraction(interaction);
                    return true;
                }
            }
            return false;
        }

        private void triggerInteraction(InteractionObject interaction)
        {
            // Collect satisfied effects before executing - effects can change
            // flags and influence the Conditions of subsequent effects.
            List<Effect> satisfiedEffects = new List<Effect>();
            foreach (var cEffects in interaction.Effects)
            {
                if (cEffects.Condition.Satisfied)
                {
                    foreach (var effect in cEffects.Effects)
                    {
                        satisfiedEffects.Add(effect);
                    }
                }
            }
            foreach (var effect in satisfiedEffects)
            {
                effect.Execute();
            }
        }

        private void triggerEvent(EventObject ev)
        {
            List<Effect> satisfiedEffects = new List<Effect>();
            foreach (var cEffects in ev.Effects)
            {
                if (cEffects.Condition.Satisfied)
                {
                    foreach (var effect in cEffects.Effects)
                    {
                        satisfiedEffects.Add(effect);
                    }
                }
            }
            foreach (var effect in satisfiedEffects)
            {
                effect.Execute();
            }
        }
    }
}