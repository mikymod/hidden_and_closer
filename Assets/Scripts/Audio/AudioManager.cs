using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace HNC {
    public class AudioManager : MonoBehaviour 
    {
        private Pooler pooler;

        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private AudioMixerSnapshot[] audioMixerSnapShots = default;
        [Range(0f, 1f)] private float masterVolume = 1f;
        [Range(0f, 1f)] private readonly float musicVolume = 1f;
        [Range(0f, 1f)] private readonly float sfxVolume = 1f;

        public static UnityAction<AudioClipsBankSO, AudioConfigurationSO, bool, float> OnSoundPlay;
        public static UnityAction<float> OnSoundStop;
        public static UnityAction OnSoundPause;
        public static UnityAction OnSoundResume;
        public static UnityAction OnSoundFadeIn;
        public static UnityAction OnSoundFadeOut;
        public static UnityAction<float> OnMasterVolumeChanged;
        public static UnityAction<float> OnMusicVolumeChanged;
        public static UnityAction<float> OnSFXVolumeChanged;
        public static UnityAction<Transform> OnDoingNoise;

        private void Awake() => pooler = GetComponent<Pooler>();

        private void OnEnable() {
            OnSoundPlay += Play;
            OnSoundStop += Stop;
            OnSoundPause += Pause;
            OnSoundResume += Resume;
            OnMasterVolumeChanged += MasterVolumChanged;
            OnMasterVolumeChanged += MusicVolumChanged;
            OnMasterVolumeChanged += SFXVolumChanged;
        }

        private void OnDisable() {
            OnSoundPlay -= Play;
            OnSoundStop -= Stop;
            OnSoundPause -= Pause;
            OnSoundResume -= Resume;
            OnMasterVolumeChanged -= MasterVolumChanged;
            OnMasterVolumeChanged -= MusicVolumChanged;
            OnMasterVolumeChanged -= SFXVolumChanged;
        }

        private void Play(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, bool fadeIn, float fadeTime) {
            GameObject soundEmitter = pooler.GetSoundEmitter();
            if (soundEmitter != null) {
                soundEmitter.GetComponent<SoundEmitter>().Play(audioClipBank, audioConfig, fadeIn, fadeTime);
            }
        }

        private void Stop(float fadeTime) {
            GameObject soundEmitter = pooler.DisposeSoundEmitter();
            if (soundEmitter != null) {
                soundEmitter.GetComponent<SoundEmitter>().Stop();
            }
        }

        private void Pause() {
            GameObject soundEmitter = pooler.DisposeSoundEmitter();
            if (soundEmitter != null) {
                soundEmitter.GetComponent<SoundEmitter>().Pause();
            }
        }
        private void Resume() {
            GameObject soundEmitter = pooler.DisposeSoundEmitter();
            if (soundEmitter != null) {
                soundEmitter.GetComponent<SoundEmitter>().Resume();
            }
        }
        //private void FadeIn(float fadeTime)
        //{
        //    GameObject soundEmitter = pooler.GetSoundEmitter();
        //    StartCoroutine(soundEmitter.GetComponent<SoundEmitter>().FadeIn(fadeTime));
        //}

        //private void FadeOut(float fadeTime)
        //{
        //    GameObject soundEmitter = pooler.GetSoundEmitter();
        //    StartCoroutine(soundEmitter.GetComponent<SoundEmitter>().FadeOut(fadeTime));
        //}

        private void MasterVolumChanged(float volume) {
            masterVolume = volume;
            audioMixer.SetFloat("Master", NormalizedMixerValue(volume));
        }
        private void MusicVolumChanged(float volume) {
            masterVolume = volume;
            audioMixer.SetFloat("Music", NormalizedMixerValue(volume));
        }
        private void SFXVolumChanged(float volume) {
            masterVolume = volume;
            audioMixer.SetFloat("SFX", NormalizedMixerValue(volume));
        }

        //TODO
        private float NormalizedMixerValue(float volume) => 0f;
    }
}
