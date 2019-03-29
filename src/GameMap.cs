using System;
using System.IO;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Readers;
using Microsoft.Xna.Framework;
using Serilog;
using flxkbr.unknownasofyet.state;

namespace flxkbr.unknownasofyet
{
    public class GameMap
    {
        private static Dictionary<string, GameMap> maps;

        public LayeredConsole MapConsole { get; private set; }
        public MapData MapData { get; private set; }
        public int Width { get => MapConsole.Width; }
        public int Height { get => MapConsole.Height; }

        private CellSurfaceLayer collisionLayer;

        private GameMap(string mapId)
        {
            using (Stream inStream = File.Open($"Content/Xp/{mapId}.xp", FileMode.Open))
            {
                REXPaintImage img = REXPaintImage.Load(inStream);
                MapConsole = img.ToLayeredConsole();
            }
            if (MapConsole.LayerCount > 1)
            {
                collisionLayer = MapConsole.GetLayer(1);
                while (MapConsole.LayerCount > 1)
                {
                    MapConsole.RemoveLayerAt(1);
                }
            }
            MapData = GameMapDataStorage.Get(mapId);
        }

        public bool IsWalkable(int x, int y)
        {
            if (x < 0 || x >= MapConsole.Width ||
                y < 0 || y >= MapConsole.Height) return false;
            if (collisionLayer != null)
            {
                return collisionLayer.GetGlyph(x, y) == Globals.WalkableGlyph;
            }
            return true;
        }

        public bool IsWalkable(Point point)
        {
            return IsWalkable(point.X, point.Y);
        }

        public bool IsWalkable(Rectangle collider)
        {
            for (int x = collider.X; x < (collider.X + collider.Width); x++)
                for (int y = collider.Y; y < (collider.Y + collider.Height); y++)
                {
                    if (!IsWalkable(x, y)) return false;
                }
            return true;
        }

        public InteractionObject GetInteraction(Rectangle collider)
        {
            for (int x = collider.X; x < (collider.X + collider.Width); x++)
                for (int y = collider.Y; y < (collider.Y + collider.Height); y++)
                {
                    var p = new Point(x, y);
                    if (MapData.InteractionLookup.ContainsKey(p))
                    {
                        return MapData.InteractionLookup[p];
                    }
                }
            return null;
        }

        public EventObject GetEvent(Rectangle collider)
        {
            for (int x = collider.X; x < (collider.X + collider.Width); x++)
                for (int y = collider.Y; y < (collider.Y + collider.Height); y++)
                {
                    var p = new Point(x, y);
                    if (MapData.EventLookup.ContainsKey(p))
                    {
                        return MapData.EventLookup[p];
                    }
                }
            return null;
        }

        public static void LoadMaps()
        {
            if (maps != null) return;
            // TODO: implement map list?
            string[] mapList = new string[] { "entrancestreet", "toleaving" };
            maps = new Dictionary<string, GameMap>();
            foreach (var mapId in mapList)
            {
                GameMap map = new GameMap(mapId);
                maps.Add(mapId, map);
            }
            Log.Logger.Information("Loaded {NumMaps} maps", mapList.Length);
        }

        public static GameMap Get(string mapId)
        {
            if (!maps.ContainsKey(mapId))
            {
                throw new Exception($"No GameMap for mapId {mapId}");
            }
            return maps[mapId];
        }
    }
}