using HNC.Save;
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

        private bool _fullscreen = true;
        private bool _vsync = true;
        private float _masterVolume = 1;
        private float _musicVolume = 1;
        private float _sfxVolume = 1;

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
            _fullscreen = fullscreen;
            Screen.fullScreen = fullscreen;
        }

        public void ToggleVerticalSync(bool verticalSync)
        {
            _vsync = verticalSync;
            QualitySettings.vSyncCount = verticalSync ? 1 : 0;
        }

        public void ChangeMasterVolume(float volume)
        {
            _masterVolume = volume;
            AudioManager.OnMasterVolumeChanged?.Invoke(volume);
        }

        public void ChangeMusicVolume(float volume)
        {
            _musicVolume = volume;
            AudioManager.OnMusicVolumeChanged?.Invoke(volume);
        }

        public void ChangeSFXVolume(float volume)
        {
            _sfxVolume = volume;
            AudioManager.OnSFXVolumeChanged?.Invoke(volume);
        }

        public void SaveAndBack()
        {
            SaveSystem.GraphicSettingsSave?.Invoke(_fullscreen, _vsync);
            SaveSystem.AudioSettingsSave?.Invoke(_masterVolume, _musicVolume, _sfxVolume);
            CloseScreen();
        }
    }
}
