using SadConsole;
using Microsoft.Xna.Framework;

namespace flxkbr.unknownasofyet
{
    public static class Globals
    {
        public static readonly int GameHeight = 60;
        public static readonly int GameWidth = 80;

        public static readonly int WorldBorderHeight = 47;
        public static readonly int WorldBorderWidth = 60;
        public static readonly int WorldHeight = 45;
        public static readonly int WorldWidth = 58;
        public static readonly int StatusHeight = 60;
        public static readonly int StatusWidth = 20;
        public static readonly int TextPanelHeight = 13;
        public static readonly int TextPanelWidth = 60;

        public static readonly int TextPanelY = 47;
        public static readonly int StatusPanelX = 60;

        public static readonly int MaxTextLineLength = TextPanelWidth - 4;
        public static readonly double TextWritingDuration = 500;

        public static readonly int WalkableGlyph = 'O';

        public static readonly string DialoguePath = "Content/Json/dialogue.json";
        public static readonly string MapDataPath = "Content/Json/mapdata.json";
        public static readonly string EntityDataPath = "Content/Json/entitydata.json";
        public static readonly string XpPath = "Content/Xp/";

        public static class Colors {
            public static readonly Color
                Red = new Color(172, 50, 50),
                RedLight = new Color(217, 87, 99),
                BlueNight = new Color(34, 32, 52),
                BlueDark = new Color(91, 110, 225),
                Blue = new Color(99, 155, 255),
                BlueLight = new Color(95, 205, 228),
                Purple = new Color(118, 66, 138),
                White = Color.White,
                Black = Color.Black,
                BrownDark = new Color(102, 57, 49),
                Brown = new Color(143, 86, 59),
                GreenTurqouise = new Color(55, 148, 110);

        }
    }
}