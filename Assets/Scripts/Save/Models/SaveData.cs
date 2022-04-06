using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HNC.Save
{
    public class SaveData
    {
        public Player Player { get; set; } = new Player();
        public List<Level> Levels { get; set; } = new List<Level>();
        public Settings Settings { get; set; } = new Settings();
        public DateTime CreatedAt { get; set; } = default;
        public DateTime UpdatedAt { get; set; } = default;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<SaveData>(json);
    }
}