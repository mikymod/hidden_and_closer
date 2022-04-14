using HNC.Audio;
using HNC.Save;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HNC
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private InputHandler input = default;
        [SerializeField] private GameObject firstElement; // Required by EventSystem
        [SerializeField] private SaveSystem saveSystem;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Toggle fullScreenToogle;
        [SerializeField] private Toggle vSyncToogle;

        //public static UnityAction RestoreUI;

        public UnityAction SettingsClosed;

        private bool _fullscreen = true;
        private bool _vsync = true;
        private float _masterVolume = 1;
        private float _musicVolume = 1;
        private float _sfxVolume = 1;

        private void OnEnable()
        {
            input.pause += CloseScreen;
            fullScreenToogle.isOn = saveSystem.SaveData.Settings.Graphic.Fullscreen;
            vSyncToogle.isOn = saveSystem.SaveData.Settings.Graphic.VerticalSync;
            masterSlider.value = saveSystem.SaveData.Settings.Audio.Master;
            SFXSlider.value = saveSystem.SaveData.Settings.Audio.SFX;
            musicSlider.value = saveSystem.SaveData.Settings.Audio.Music;
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
            AudioEventsManager.OnMasterVolumeChanged?.Invoke(volume);
        }

        public void ChangeMusicVolume(float volume)
        {
            _musicVolume = volume;
            AudioEventsManager.OnMusicVolumeChanged?.Invoke(volume);
        }

        public void ChangeSFXVolume(float volume)
        {
            _sfxVolume = volume;
            AudioEventsManager.OnSFXVolumeChanged?.Invoke(volume);
        }

        public void SaveAndBack()
        {
            SaveSystem.GraphicSettingsSave?.Invoke(_fullscreen, _vsync);
            SaveSystem.AudioSettingsSave?.Invoke(_masterVolume, _musicVolume, _sfxVolume);
            CloseScreen();
        }
    }
}
