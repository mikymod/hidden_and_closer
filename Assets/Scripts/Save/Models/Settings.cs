using Newtonsoft.Json;

namespace HNC.Save
{
    public class Settings
    {
        public GraphicSettings Graphic { get; set; } = new GraphicSettings();
        public AudioSettings Audio { get; set; } = new AudioSettings();

        public string ToJson() => JsonConvert.SerializeObject(this);
        public Settings FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json);
    }

    public class GraphicSettings
    {
        public bool Fullscreen { get; set; } = true;
        public bool VerticalSync { get; set; } = true;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public GraphicSettings FromJson(string json) => JsonConvert.DeserializeObject<GraphicSettings>(json);
    }

    public class AudioSettings
    {
        public float Master { get; set; } = 1f;
        public float Music { get; set; } = 1f;
        public float SFX { get; set; } = 1f;

        public string ToJson() => JsonConvert.SerializeObject(this);
        public static AudioSettings FromJson(string json) => JsonConvert.DeserializeObject<AudioSettings>(json);
    }
}