using System;
using SadConsole;

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

        public GameScreen()
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
            TextPanelActive = true;
            WorldPanelActive = true;
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (TextPanelActive)
            {
                textPanel.HandleInput(info);
            }
            if (WorldPanelActive)
            {
                world.Player.HandleInput(info);
                world.HandleInput(info);
            }
            return false;
        }

        private void onTextPanelDialogCompleted(object sender, EventArgs args)
        {
            TextPanelActive = false;
        }
    }
}