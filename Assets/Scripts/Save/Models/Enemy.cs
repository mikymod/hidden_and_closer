using Newtonsoft.Json;
using UnityEngine;

namespace HNC.Save
{
    public class Enemy
    {
        public string Type { get; set; }
        public Vector3 Position { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<Enemy>(json);
    }
}