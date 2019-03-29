using System;
using System.Collections.Generic;
using SadConsole;
using Microsoft.Xna.Framework;
using Serilog;
using Serilog.Events;
using flxkbr.unknownasofyet.text;
using flxkbr.unknownasofyet.state;

namespace flxkbr.unknownasofyet
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            SadConsole.Settings.AllowWindowResize = false;
            SadConsole.Settings.UnlimitedFPS = true;
            
            SadConsole.Game.Create("Content/Fonts/Cheepicus12.font", 80, 60);

            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            // var console = new SadConsole.Console(80, 25);
            // console.FillWithRandomGarbage();
            // console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
            // console.Print(4, 4, "Hello from SadConsole");
            // SadConsole.Global.CurrentScreen = console;
            // SadConsole.Global.CurrentScreen = new SadConsole.Console(80, 60);
            DialogStorage.Init();
            EntityDataStorage.Init();
            GameMapDataStorage.Init();
            GameMap.LoadMaps();

            SadConsole.Global.CurrentScreen = new ContainerConsole();
            var gameScreen = new GameScreen();
            SadConsole.Global.CurrentScreen.Children.Add(gameScreen);
            SadConsole.Global.FocusedConsoles.Set(gameScreen);

        }

        static void Update(GameTime gameTime)
        {
            var pressedKeys = SadConsole.Global.KeyboardState.KeysPressed;
            // if (pressedKeys.Count > 0)
            // {
            //     System.Console.WriteLine("Keys Pressed:");
            //     foreach (var key in pressedKeys)
            //     {
            //         System.Console.WriteLine(key.Key.ToString());
            //     }
                
            // }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                SadConsole.Game.Instance.Exit();
            }
        }

        static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("full.log")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();
        }
    }
}
