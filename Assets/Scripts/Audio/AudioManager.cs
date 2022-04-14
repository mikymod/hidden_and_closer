using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace HNC.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private Pooler pooler;

        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private InputHandler input;
        [Range(0f, 1f)] private float masterVolume = 1f;
        [Range(0f, 1f)] private float musicVolume = 1f;
        [Range(0f, 1f)] private float sfxVolume = 1f;
        private bool filtered = false;

        private void Awake()
        {
            //DontDestroyOnLoad(transform.gameObject);
            pooler = GetComponent<Pooler>();
        }
        private void OnEnable()
        {
            AudioEventsManager.OnSoundPlay += Play;
            AudioEventsManager.OnSoundStop += Stop;
            AudioEventsManager.OnSoundPause += Pause;
            AudioEventsManager.OnSoundResume += Resume;
            AudioEventsManager.OnMasterVolumeChanged += MasterVolumChanged;
            AudioEventsManager.OnMusicVolumeChanged += MusicVolumChanged;
            AudioEventsManager.OnSFXVolumeChanged += SFXVolumChanged;
            AudioEventsManager.OnFadeIn +=FadeIn;
            AudioEventsManager.OnFadeOut += FadeOut;
            //input.pause += ChangeAudioMixerSnapshot;
        }

        private void OnDisable()
        {
            AudioEventsManager.OnSoundPlay -= Play;
            AudioEventsManager.OnSoundStop -= Stop;
            AudioEventsManager.OnSoundPause -= Pause;
            AudioEventsManager.OnSoundResume -= Resume;
            AudioEventsManager.OnMasterVolumeChanged -= MasterVolumChanged;
            AudioEventsManager.OnMusicVolumeChanged -= MusicVolumChanged;
            AudioEventsManager.OnSFXVolumeChanged -= SFXVolumChanged;
            AudioEventsManager.OnFadeIn -= FadeIn;
            AudioEventsManager.OnFadeOut -= FadeOut;
            //input.pause -= ChangeAudioMixerSnapshot;
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
            audioMixer.SetFloat("Master", NormalizedMixerValue(volume));
        }
        private void MusicVolumChanged(float volume)
        {
            musicVolume = volume;
            audioMixer.SetFloat("Music", NormalizedMixerValue(volume));
        }
        private void SFXVolumChanged(float volume)
        {
            sfxVolume = volume;
            audioMixer.SetFloat("SFX", NormalizedMixerValue(volume));
        }

        private float NormalizedMixerValue(float normalizedValue) => (normalizedValue - 1f) * 80f; 

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


        //Non credo serva
        //public IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
        //{
        //    float time = 0f;
        //    float startVolume = audioSource.volume;
        //    float duration = fadeTime;

        //    while (time < duration)
        //    {
        //        audioSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
        //        time += Time.deltaTime;
        //        yield return null;
        //    }

        //    audioSource.volume = 1f;
        //}

        //Non credo serva
        //public IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        //{
        //    float time = 0f;
        //    float startVolume = audioSource.volume;
        //    float duration = fadeTime;

        //    while (time < duration)
        //    {
        //        audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
        //        time += Time.deltaTime;
        //        yield return null;
        //    }

        //    audioSource.volume = 0f;
        //    audioSource.Stop();
        //}
    
        private void ChangeAudioMixerSnapshot()
        {
            if (filtered)
            {
                filtered = false;
                audioMixer.FindSnapshot("Default").TransitionTo(1f);
            }
            else
            {
                filtered = true;
                audioMixer.FindSnapshot("Filtered").TransitionTo(1f);
            }
        }
    }
}
