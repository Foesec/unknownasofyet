using SadConsole;
using Microsoft.Xna.Framework;

namespace flxkbr.unknownasofyet
{
    public static class Utils
    {
        public static void CreateBorder(Console console, Color borderColor, Color backgroundColor) {
            ColoredGlyph hBorder = new ColoredGlyph(196, borderColor, backgroundColor);
            ColoredGlyph vBorder = new ColoredGlyph(179, borderColor, backgroundColor);
            ColoredGlyph corner = new ColoredGlyph(234, borderColor, backgroundColor);
            for (int x = 1;  x < console.Width - 1;  x++)
            {
                console.Print(x, 0, hBorder);
                console.Print(x, console.Height - 1, hBorder);
            }
            for (int y = 1; y < console.Height - 1; y++)
            {
                console.Print(0, y, vBorder);
                console.Print(console.Width - 1, y, vBorder);
            }
            console.Print(0, 0, corner);
            console.Print(console.Width - 1, 0, corner);
            console.Print(0, console.Height - 1, corner);
            console.Print(console.Width - 1, console.Height - 1, corner);
        }
    }
}