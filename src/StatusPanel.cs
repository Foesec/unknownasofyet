
using SadConsole;
using Microsoft.Xna.Framework;

namespace flxkbr.unknownasofyet
{
    public class StatusPanel : Console
    {
        readonly Color BorderColor = Globals.Colors.Brown;
        readonly Color BackgroundColor = Globals.Colors.Black;
        public StatusPanel() : base(Globals.StatusWidth, Globals.StatusHeight)
        {
            this.Position = new Point(Globals.StatusPanelX, 0);
            Utils.CreateBorder(this, BorderColor, BackgroundColor);
        }

        public void WriteLabels()
        {
            this.Print(2, 2, "Health", Globals.Colors.GreenTurqouise);
            this.Print(3, 4, "100 / 100", Globals.Colors.GreenTurqouise);

            this.Print(2, 7, "Mental", Globals.Colors.Purple);
            this.Print(3, 9, "78%", Globals.Colors.Purple);

            this.Print(2, 12, "Insight", Globals.Colors.White);
            this.Print(3, 14, "3", Globals.Colors.White);
        }
    }
}