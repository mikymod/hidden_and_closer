using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace HNC
{
    public class AudioManager : MonoBehaviour
    {
        private Pooler pooler;

        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private AudioMixerSnapshot audioMixerSnapshots = default;
        [Range(0f, 1f)] private float masterVolume = 1f;
        [Range(0f, 1f)] private readonly float musicVolume = 1f;
        [Range(0f, 1f)] private readonly float sfxVolume = 1f;

        public static UnityAction<AudioClipsBankSO, AudioConfigurationSO, Transform, float> OnSoundPlay;
        public static UnityAction<AudioClipsBankSO, Transform, float> OnSoundStop;
        public static UnityAction<AudioClipsBankSO, float> OnSoundPause;
        public static UnityAction<AudioClipsBankSO, float> OnSoundResume;
        public static UnityAction<float> OnMasterVolumeChanged;
        public static UnityAction<float> OnMusicVolumeChanged;
        public static UnityAction<float> OnSFXVolumeChanged;

        private void Awake()
        {
            pooler = GetComponent<Pooler>();
            ChangeAudioMixerSnapshot("Default");
        }
        private void OnEnable()
        {
            OnSoundPlay += Play;
            OnSoundStop += Stop;
            OnSoundPause += Pause;
            OnSoundResume += Resume;
            OnMasterVolumeChanged += MasterVolumChanged;
            OnMasterVolumeChanged += MusicVolumChanged;
            OnMasterVolumeChanged += SFXVolumChanged;
        }

        private void OnDisable()
        {
            OnSoundPlay -= Play;
            OnSoundStop -= Stop;
            OnSoundPause -= Pause;
            OnSoundResume -= Resume;
            OnMasterVolumeChanged -= MasterVolumChanged;
            OnMasterVolumeChanged -= MusicVolumChanged;
            OnMasterVolumeChanged -= SFXVolumChanged;
        }

        private void Play(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, Transform transform, float fadeTime)
        {
            GameObject soundEmitter = pooler.Get();
            if (soundEmitter != null)
            {
                soundEmitter.GetComponent<SoundEmitter>().Play(audioClipBank, audioConfig, transform, fadeTime);
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

        private void MasterVolumChanged(float volume)
        {
            masterVolume = volume;
            audioMixer.SetFloat("Master", NormalizedMixerValue(volume));
        }
        private void MusicVolumChanged(float volume)
        {
            masterVolume = volume;
            audioMixer.SetFloat("Music", NormalizedMixerValue(volume));
        }
        private void SFXVolumChanged(float volume)
        {
            masterVolume = volume;
            audioMixer.SetFloat("SFX", NormalizedMixerValue(volume));
        }

        private float NormalizedMixerValue(float normalizedValue) => (normalizedValue - 1f) * 80f;

        public IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 1f;
        }

        public IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
        }

        private void ChangeAudioMixerSnapshot(string snapshotName) => audioMixerSnapshots.TransitionTo(0f);
    }
}
