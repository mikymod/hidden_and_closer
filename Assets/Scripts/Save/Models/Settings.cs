using Newtonsoft.Json;

namespace HNC.Save
{
    public class Settings
    {
        public GraphicSettings Graphic { get; set; }
        public AudioSettings Audio { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json);
    }

    public class GraphicSettings
    {
        public bool Fullscreen { get; set; }
        public bool VerticalSync { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<GraphicSettings>(json);
    }

    public class AudioSettings
    {
        public float Master { get; set; }
        public float Music { get; set; }
        public float SFX { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<AudioSettings>(json);
    }
}