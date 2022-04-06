using Newtonsoft.Json;

namespace HNC.Save
{
    public class Light
    {
        public string Type { get; set; }
        public bool Enabled { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<Light>(json);
    }
}