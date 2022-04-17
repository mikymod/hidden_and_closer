using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HNC.Audio
{
    public class AudioMainMenuMusicController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO mainMenuMusicClipBank;
        [SerializeField] private AudioConfigurationSO mainMenuMusicConfiguration;

        private void Start()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(mainMenuMusicClipBank, mainMenuMusicConfiguration, null, 1f);
        }
    }
}
