using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioLampController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO buzzingClipsBank;
        [SerializeField] private AudioConfigurationSO buzzingConfiguration;
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;
        [SerializeField] private AudioClipsBankSO brokenGlassClipsBank;
        [SerializeField] private AudioConfigurationSO brokenGlassConfiguration;
      
        public void PlayBuzzingSound() => AudioManager.OnSoundPlay?.Invoke(buzzingClipsBank, buzzingConfiguration, true, 0f);
        public void StopBuzzingSound() => AudioManager.OnSoundStop?.Invoke(0f);
        public void PlayHitSound() => AudioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, false, 0f);
        public void PlayBrokenGlassSound()
        {
            AudioManager.OnSoundPlay?.Invoke(brokenGlassClipsBank, brokenGlassConfiguration, false, 0f);
            AudioManager.OnDoingNoise?.Invoke(transform);
        }
    }
}
