using Newtonsoft.Json;
using UnityEngine;

namespace HNC.Save
{
    public class Player
    {
        public int Ammo { get; set; } = default;
        public string Scene { get; set; } = default;
        public Vector3 Position { get; set; } = default;
        public bool CompanionAvailable { get; set; } = default;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<Player>(json);
    }
}

