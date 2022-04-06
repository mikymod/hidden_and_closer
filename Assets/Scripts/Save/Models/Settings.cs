using Newtonsoft.Json;

namespace HNC.Save
{
    public class Settings
    {
        public GraphicSettings Graphic { get; set; } = new GraphicSettings();
        public AudioSettings Audio { get; set; } = new AudioSettings();

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json);
    }

    public class GraphicSettings
    {
        public bool Fullscreen { get; set; } = default;
        public bool VerticalSync { get; set; } = default;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<GraphicSettings>(json);
    }

    public class AudioSettings
    {
        public float Master { get; set; } = default;
        public float Music { get; set; } = default;
        public float SFX { get; set; } = default;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public void FromJson(string json) => JsonConvert.DeserializeObject<AudioSettings>(json);
    }
}