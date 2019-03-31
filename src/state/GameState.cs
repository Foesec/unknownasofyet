using System.Collections.Generic;
using Serilog;

namespace flxkbr.unknownasofyet.state
{
    public class GameState
    {
        private static Dictionary<string, bool> flags = new Dictionary<string, bool>();

        // public static void Init()
        // {
        //     flags = new Dictionary<string, bool>();
        // }

        public static bool GetFlag(string flag)
        {
            if (flags.ContainsKey(flag))
            {
                return flags[flag];
            }
            flags.Add(flag, false);
            return false;
        }

        public static void SetFlag(string flag, bool predicate)
        {
            if (flags.ContainsKey(flag))
            {
                flags[flag] = predicate;
            }
            else
            {
                flags.Add(flag, predicate);
            }
        }

        public static bool IsSatisfied(Condition c)
        {
            bool flag = GetFlag(c.Flag);
            return (flag == c.Predicate);
        }

        public static Dictionary<string, bool> GetSnapshot(List<string> flags)
        {
            Dictionary<string, bool> snapshot = new Dictionary<string, bool>(flags.Count);
            foreach (var flag in flags)
            {
                snapshot.TryAdd(flag, GetFlag(flag));
            }
            return snapshot;
        }
    }
}