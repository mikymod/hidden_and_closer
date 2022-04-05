using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HNC
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private InputHandler input = default;
        [SerializeField] private GameObject firstElement; // Required by EventSystem

        public UnityAction SettingsClosed;

        private void OnEnable()
        {
            input.pause += CloseScreen;
        }

        private void OnDisable()
        {
            input.pause -= CloseScreen;
        }

        private void CloseScreen()
        {
            SettingsClosed?.Invoke();
        }

        public void InitSettingsMenu()
        {
            EventSystem.current.SetSelectedGameObject(firstElement);
        }

        public void ToggleFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        public void ToggleVerticalSync(bool verticalSync)
        {
            QualitySettings.vSyncCount = verticalSync ? 1 : 0;
        }

        public void ChangeMasterVolume(float volume) => AudioManager.OnMasterVolumeChanged?.Invoke(volume);
        public void ChangeMusicVolume(float volume) => AudioManager.OnMusicVolumeChanged?.Invoke(volume);
        public void ChangeSFXVolume(float volume) => AudioManager.OnSFXVolumeChanged?.Invoke(volume);

        public void SaveAndBack()
        {
            CloseScreen();
        }
    }
}
