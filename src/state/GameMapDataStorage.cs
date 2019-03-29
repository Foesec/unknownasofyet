using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace flxkbr.unknownasofyet.state
{
    public class GameMapDataStorage
    {
        private Dictionary<string, MapData> mapData;

        private static GameMapDataStorage instance;

        public static bool Initialized
        {
            get => (instance != null);
        }

        private GameMapDataStorage()
        {
            mapData = JsonConvert.DeserializeObject<Dictionary<string, MapData>>(
                File.ReadAllText(Globals.MapDataPath),
                new ConditionConverter());
        }

        public static void Init()
        {
            if (instance == null)
            {
                instance = new GameMapDataStorage();
            }
        }

        public static MapData Get(string mapId)
        {
            if (instance == null)
            {
                throw new Exception("GameMapDataStorage has not been initialized yet.");
            }
            if (!instance.mapData.ContainsKey(mapId))
            {
                // throw new Exception($"Map with id {mapId} not found in GameMapDataStorage");
                Console.Error.WriteLine($"No GameMapData for map {mapId}");
                return null;
            }
            return instance.mapData[mapId];
        }
    }

    public class MapData
    {
        public Point Spawn { get; set; }
        public Player.RSize PlayerSize { get; set; }
        public Dictionary<Point, EventObject> EventLookup { get; private set; }
        public Dictionary<Point, InteractionObject> InteractionLookup { get; private set; }

        private List<EventObject> events;
        private List<InteractionObject> interactions;

        public List<ConnectionObject> Connections { get; set; }
        public List<EventObject> Events
        { 
            get => events;
            set
            {
                events = value;
                EventLookup = new Dictionary<Point, EventObject>();
                foreach (var ev in events)
                {
                    foreach (var loc in ev.Locations)
                    {
                        if (EventLookup.ContainsKey(loc))
                        {
                            Console.Error.Write($"EventLookup contains multiple events at loc {loc}");
                        }
                        else
                        {
                            EventLookup.Add(loc, ev);
                        }
                    }
                }
            }
        }
        public List<InteractionObject> Interactions
        { 
            get => interactions;
            set
            {
                interactions = value;
                InteractionLookup = new Dictionary<Point, InteractionObject>();
                foreach (var interaction in interactions)
                {
                    foreach (var loc in interaction.Locations)
                    {
                        if (InteractionLookup.ContainsKey(loc))
                        {
                            Console.Error.WriteLine($"InteractionLookup contains multiple interactions at loc {loc}");
                        }
                        else
                        {
                            InteractionLookup.Add(loc, interaction);
                        }
                    }
                }
            } 
        }
        
    }

    public class ConnectionObject
    {
        public string To { get; set; }
        public List<Point> Locations { get; set; }
        public Condition Condition { get; set; }

        public override string ToString()
        {
            return $"{{Connection to {To} IF {Condition.ToString()}}}";
        }
    }

    public class EventObject
    {
        public string Name { get; set; }
        public Condition Condition { get; set; }
        public List<Point> Locations { get; set; }

        public override string ToString()
        {
            return $"{{{Name} IF {Condition.ToString()}}}";
        }
    }

    public class InteractionObject
    {
        public string Name { get; set; }
        public Condition Condition { get; set; }
        public string Text { get; set; }
        public List<Point> Locations { get; set; }

        public override string ToString()
        {
            return $"{{{Name} IF {Condition.ToString()}}}";
        }
    }

    public class ConditionConverter : JsonConverter<Condition>
    {
        public override Condition ReadJson(JsonReader reader, Type objectType, Condition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string cond = (string)reader.Value;
            return new Condition(cond);
        }

        public override void WriteJson(JsonWriter writer, Condition value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}