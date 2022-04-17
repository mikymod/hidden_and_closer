using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace HNC.Audio
{
    public class AudioEventsManager : ScriptableObject
    {
        public static UnityAction<AudioClipsBankSO, AudioConfigurationSO, Transform, float> OnSoundPlay;
        public static UnityAction<AudioClipsBankSO, AudioConfigurationSO, Transform, float> OnSoundPlayEsclusive;
        public static UnityAction<AudioClipsBankSO, Transform, float> OnSoundStop;
        public static UnityAction<AudioClipsBankSO, float> OnSoundPause;
        public static UnityAction<AudioClipsBankSO, float> OnSoundResume;
        public static UnityAction<float> OnMasterVolumeChanged;
        public static UnityAction<float> OnMusicVolumeChanged;
        public static UnityAction<float> OnSFXVolumeChanged;
        public static UnityAction<AudioClipsBankSO, float> OnFadeIn;
        public static UnityAction<AudioClipsBankSO, float> OnFadeOut;
    }
}
