using System;
using System.Collections.Generic;
using System.Linq;
using SadConsole;
using Serilog;
using flxkbr.unknownasofyet.text;

namespace flxkbr.unknownasofyet
{
    public class GameScreen : ContainerConsole
    {
        WorldPanel world;
        StatusPanel statusPanel;
        SadConsole.Console worldBorder;
        public TextPanel textPanel;

        public bool TextPanelActive { get; private set; }
        public bool WorldPanelActive { get; private set; }

        private bool interactionDisabled = false;
        private double reactivationTimer = Globals.ReactivationDelay;

        private static GameScreen instance;

        public static void Init()
        {
            if (instance == null) instance = new GameScreen();
        }

        public static GameScreen GetInstance()
        {
            if (instance == null)
            {
                throw new NullReferenceException("GameScreen hast not been initialized");
            }
            return instance;
        }

        private GameScreen()
        {
            this.world = new WorldPanel();
            worldBorder = new SadConsole.Console(Globals.WorldBorderWidth, Globals.WorldBorderHeight);
            Utils.CreateBorder(worldBorder, Globals.Colors.White, Globals.Colors.BlueNight);
            this.statusPanel = new StatusPanel();
            this.textPanel = new TextPanel();

            this.textPanel.DialogsCompleted += onTextPanelDialogCompleted;

            this.Children.Add(this.worldBorder);
            this.Children.Add(this.world);
            this.Children.Add(this.statusPanel);
            this.Children.Add(this.textPanel);

            world.LoadLevel("entrancestreet");

            // this.statusPanel.WriteLabels();
            TextPanelActive = false;
            WorldPanelActive = true;
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (interactionDisabled) return false;
            if (TextPanelActive && !interactionDisabled)
            {
                textPanel.HandleInput(info);
            }
            if (WorldPanelActive && !interactionDisabled)
            {
                
                world.Player.HandleInput(info);
                world.HandleInput(info);
            }
            return false;
        }

        public override void Update(TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
            if (interactionDisabled) handleReactivationTimer(timeElapsed);
        }

        public void WriteDialogs(List<string> dialogueIds)
        {
            if (TextPanelActive)
            {
                Log.Logger.Error("Trying to call WriteDialogs while TextPanelActive is true");
                return;
            }
            TextPanelActive = true;
            WorldPanelActive = false;
            var dialogues = dialogueIds.Select(id => DialogStorage.Get(id)).ToList();
            if (dialogues.Count() == 1)
            {
                textPanel.WriteDialog(dialogues[0]);
            }
            else
            {
                textPanel.WriteDialogs(dialogues);
            }
        }

        private void handleReactivationTimer(TimeSpan elapsed)
        {
            if (reactivationTimer <= elapsed.TotalMilliseconds)
            {
                reactivationTimer = Globals.ReactivationDelay;
                interactionDisabled = false;
            }
            else
            {
                reactivationTimer -= elapsed.TotalMilliseconds;
            }
        }

        private void onTextPanelDialogCompleted(object sender, EventArgs args)
        {
            interactionDisabled = true;
            TextPanelActive = false;
            WorldPanelActive = true;
        }
    }
}