using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class MainMenuMusicController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO mainMenuMusicClipBank;
        [SerializeField] private AudioConfigurationSO mainMenuMusicConfiguration;

        private void Start()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(mainMenuMusicClipBank, mainMenuMusicConfiguration, null, 1f);
        }
    }
}
