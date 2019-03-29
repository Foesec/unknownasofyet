using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace flxkbr.unknownasofyet.text
{
    public class DialogStorage
    {
        private static DialogStorage instance;
        
        private Dictionary<string, DialogUnit> dialogs;

        private DialogStorage() {}

        private void initStorage()
        {
            var rawDialogs = JsonConvert
                .DeserializeObject<Dictionary<string, DialogObject>>(
                    File.ReadAllText(Globals.DialoguePath));
            this.dialogs = new Dictionary<string, DialogUnit>();
            foreach (var item in rawDialogs)
            {
                bool success = this.dialogs.TryAdd(item.Key, new DialogUnit(item.Value));
                if (!success)
                {
                    Console.Error.WriteLineAsync($"DialogStorage.initStorage(): Duplicate key {item.Key} detected");
                }
            }
        }

        public static void Init()
        {
            if (instance != null) return;
            instance = new DialogStorage();
            instance.initStorage();
        }

        public static DialogUnit Get(string dialogId)
        {
            if (instance == null)
            {
                throw new NullReferenceException("DialogStorage has not been initialized yet.");
            }
            return instance.dialogs[dialogId];
        }
    }
}