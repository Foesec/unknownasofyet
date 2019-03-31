using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Serilog;

namespace flxkbr.unknownasofyet.state
{
    public class EntityDataStorage
    {
        private static EntityDataStorage instance;

        protected Dictionary<string, EntityData> storage;

        private EntityDataStorage()
        {
            this.storage = JsonConvert
                .DeserializeObject<Dictionary<string, EntityData>>(
                    File.ReadAllText(Globals.EntityDataPath));
        }

        public static void Init()
        {
            if (instance == null)
            {
                instance = new EntityDataStorage();
                Log.Logger.Information("Successfully initialized EntityDataStorage");
            }
        }

        public static EntityData Get(string entityId)
        {
            if (instance == null)
            {
                throw new NullReferenceException(
                    "EntityDataStorage has not been initialized yet");
            }
            if (!instance.storage.ContainsKey(entityId))
            {
                throw new KeyNotFoundException($"No EntityData for entity {entityId}");
            }
            return instance.storage[entityId];
        }
    }

    public class EntityData
    {
        public string DisplayName { get; set; }
        public float AnimDuration { get; set; }
        public bool Repeat { get; set; }
        public bool Bounce { get; set; }
        public Point Dimensions { get; set; }
        public Rectangle Collider { get; set; }
    }
}