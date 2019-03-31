using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Serilog;

namespace flxkbr.unknownasofyet.state
{
    public abstract class Effect
    {
        private string desc;

        public string EffectType { get; protected set; }

        public Effect(string desc)
        {
            this.desc = desc;
        }

        public abstract void Execute();

        public override string ToString()
        {
            return desc;
        }
    }

    // Format: "T:id1,id2..."
    public class TextEffect : Effect
    {
        public List<string> DialogueIDs { get; }

        public TextEffect(string desc) : base(desc)
        {
            this.EffectType = "text";
            if (!desc.StartsWith("T:")) throw new ArgumentException($"Invalid description {desc} for TextEffect");
            string idsString = desc.Split(':')[1];
            this.DialogueIDs = new List<string>(idsString.Split(","));
        }

        public override void Execute()
        {
            Log.Logger.Debug("Executing TextEffect {Effect}", this);
            GameScreen gs = GameScreen.GetInstance();
            gs.WriteDialogs(DialogueIDs);
        }
    }

    public class FlagEffect : Effect
    {
        public string Flag { get; }
        public bool Predicate { get; }

        public FlagEffect(string desc) : base(desc)
        {
            this.EffectType = "flag";
            if (!desc.StartsWith("F:")) throw new ArgumentException($"Invalid description {desc} for FlagEffect");
            string flagString = desc.Split(':')[1];
            if (flagString.StartsWith('!'))
            {
                Predicate = false;
                Flag = flagString.Substring(1);
            }
            else
            {
                Predicate = true;
                Flag = flagString;
            }
        }

        public override void Execute()
        {
            GameState.SetFlag(Flag, Predicate);
        }
    }

    public class EffectConverter : JsonConverter<Effect>
    {
        public override Effect ReadJson(JsonReader reader, Type objectType, Effect existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string desc = (string)reader.Value;
            if (desc.StartsWith("T:"))
            {
                return new TextEffect(desc);
            }
            if (desc.StartsWith("F:"))
            {
                return new FlagEffect(desc);
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Effect value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}