using UnityEngine;

public class LightDetector : MonoBehaviour
{
    [SerializeField] private RenderTexture rt;
    [SerializeField][Range(0f, 0.5f)] private float lightThreshold = 0.05f;
    private Texture2D texture;

    public bool illuminated = false;
    public bool Illuminated { get => illuminated; }

    private void Start()
    {
        texture = new Texture2D(32, 32, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
        var pixels = texture.GetPixels(0, 0, 32, 32, 0);
        var avarageLuminosity = AvarageLuminosity(pixels);
        illuminated = avarageLuminosity > lightThreshold;
    }

    private float AvarageLuminosity(Color[] colors)
    {
        float r = 0f;
        foreach (var color in colors)
        {
            r += color.r;
        }

        return r / colors.Length;
    }
}
