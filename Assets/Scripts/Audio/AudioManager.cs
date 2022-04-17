using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HNC.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private Pooler pooler;

        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private SaveSystem saveSystem;
        [Range(0f, 1f)] private float masterVolume = 1f;
        [Range(0f, 1f)] private float musicVolume = 1f;
        [Range(0f, 1f)] private float sfxVolume = 1f;
        private bool filtered = false;


        private void Awake()
        {
            pooler = GetComponent<Pooler>();
            masterVolume = saveSystem.SaveData.Settings.Audio.Master;
            musicVolume = saveSystem.SaveData.Settings.Audio.Music;
            sfxVolume = saveSystem.SaveData.Settings.Audio.SFX;
            audioMixer.FindSnapshot("Default").TransitionTo(0f);
        }

        private void OnEnable()
        {
            AudioEventsManager.OnSoundPlay += Play;
            AudioEventsManager.OnSoundPlayEsclusive += PlayEsclusive;
            AudioEventsManager.OnSoundStop += Stop;
            AudioEventsManager.OnSoundPause += Pause;
            AudioEventsManager.OnSoundResume += Resume;
            AudioEventsManager.OnMasterVolumeChanged += MasterVolumChanged;
            AudioEventsManager.OnMusicVolumeChanged += MusicVolumChanged;
            AudioEventsManager.OnSFXVolumeChanged += SFXVolumChanged;
            AudioEventsManager.OnFadeIn +=FadeIn;
            AudioEventsManager.OnFadeOut += FadeOut;
        }


        private void OnDisable()
        {
            AudioEventsManager.OnSoundPlay -= Play;
            AudioEventsManager.OnSoundPlayEsclusive -= PlayEsclusive;
            AudioEventsManager.OnSoundStop -= Stop;
            AudioEventsManager.OnSoundPause -= Pause;
            AudioEventsManager.OnSoundResume -= Resume;
            AudioEventsManager.OnMasterVolumeChanged -= MasterVolumChanged;
            AudioEventsManager.OnMusicVolumeChanged -= MusicVolumChanged;
            AudioEventsManager.OnSFXVolumeChanged -= SFXVolumChanged;
            AudioEventsManager.OnFadeIn -= FadeIn;
            AudioEventsManager.OnFadeOut -= FadeOut;
        }

        private void Play(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, Transform transform, float fadeTime)
        {
            GameObject soundEmitter = pooler.Get();
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().Play(audioClipBank, audioConfig, transform, fadeTime);
            }
            else
            {
                soundEmitter.SetActive(false);
            }
        }

        private void PlayEsclusive(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, Transform transform, float fadeTime)
        {
            GameObject soundEmitter = pooler.Get();
            if (soundEmitter != null && !IsPlaying(audioClipBank))
            {
                soundEmitter.GetComponent<SoundEmitter>().Play(audioClipBank, audioConfig, transform, fadeTime);
            }
            else
            {
                soundEmitter.SetActive(false);
            }
        }

        private void Stop(AudioClipsBankSO audioClipBank, Transform transform, float fadeTime)
        {
            GameObject soundEmitter = pooler.DisposeSoundEmitter(audioClipBank);
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().Stop(transform, fadeTime);
            }
        }

        private void Pause(AudioClipsBankSO audioClipBank, float fadeTime)
        {
            GameObject soundEmitter = pooler.DisposeSoundEmitter(audioClipBank);
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().Pause(fadeTime);
            }
        }
        private void Resume(AudioClipsBankSO audioClipBank, float fadeTime)
        {
            GameObject soundEmitter = pooler.DisposeSoundEmitter(audioClipBank);
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().Resume(fadeTime);
            }
        }

        private bool IsPlaying(AudioClipsBankSO audioClipBank)
        {
            return pooler.IsPlaying(audioClipBank);
        }

        private void MasterVolumChanged(float volume)
        {
            masterVolume = volume;
            audioMixer.SetFloat("Master", NormalizedMixerValue2(volume));
        }
        private void MusicVolumChanged(float volume)
        {
            musicVolume = volume;
            audioMixer.SetFloat("Music", NormalizedMixerValue2(volume));
        }
        private void SFXVolumChanged(float volume)
        {
            sfxVolume = volume;
            audioMixer.SetFloat("SFX", NormalizedMixerValue2(volume));
        }

        private float NormalizedMixerValue(float normalizedValue) => (normalizedValue - 1f) * 80f; 
        private float NormalizedMixerValue2(float normalizedValue) => Mathf.Log10(normalizedValue) * 20f; 

        private void FadeIn(AudioClipsBankSO audioClipBank, float fadeTime)
        {
            GameObject soundEmitter = pooler.DisposeSoundEmitter(audioClipBank);
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().CallFadeIn(fadeTime);
            }
        }

        private void FadeOut(AudioClipsBankSO audioClipBank, float fadeTime)
        {
            GameObject soundEmitter = pooler.DisposeSoundEmitter(audioClipBank);
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().CallFadeOutWithoutStop(fadeTime);
            }
        }

        private void ChangeAudioMixerSnapshot()
        {
            if (filtered)
            {
                filtered = false;
                audioMixer.FindSnapshot("Default").TransitionTo(0f);
            }
            else
            {
                filtered = true;
                audioMixer.FindSnapshot("Filtered").TransitionTo(0f);
            }
        }


        
    }
}
