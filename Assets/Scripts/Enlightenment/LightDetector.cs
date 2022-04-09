using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class LightDetector : MonoBehaviour
    {
        [SerializeField] private RenderTexture rt;
        [SerializeField][Range(0f, 0.5f)] private float lightThreshold = 0.05f;
        private Texture2D texture;

        private bool newValueIlluminated = false;
        private bool illuminated = false;

        private void Start()
        {
            texture = new Texture2D(32, 32, TextureFormat.RGB24, false);
            CheckForLight();
        }

        private void OnEnable()
        {
            CompanionController.OnCompanionControlFinish += CheckForLight;
        }

        private void OnDisable()
        {
            CompanionController.OnCompanionControlFinish -= CheckForLight;
        }

        private void Update()
        {
            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
            var pixels = texture.GetPixels(0, 0, 32, 32, 0);
            var avarageLuminosity = AvarageLuminosity(pixels);
            illuminated = avarageLuminosity > lightThreshold;
            if (illuminated != newValueIlluminated)
            {
                CheckForLight();
            }
        
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

        private void CheckForLight()
        {
            LightingUI.OnLighting?.Invoke(illuminated);
            newValueIlluminated = illuminated;
        }
    }
}
