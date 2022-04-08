using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HNC.Save
{
    public class SaveData
    {
        public Player Player { get; set; } = new Player();
        public Dictionary<String, Level> Levels { get; set; } = new Dictionary<string, Level>();
        public Settings Settings { get; set; } = new Settings();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public static SaveData FromJson(string json) => JsonConvert.DeserializeObject<SaveData>(json);
    }
}