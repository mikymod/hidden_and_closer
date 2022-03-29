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
      
        public void PlayBuzzingSound() => AudioManager.OnSoundPlay?.Invoke(buzzingClipsBank, buzzingConfiguration, transform, 0f);
        public void StopBuzzingSound() => AudioManager.OnSoundStop?.Invoke(buzzingClipsBank, transform, 0f);
        public void PlayHitSound()
        {
            AudioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, transform, 0f);
            hitConfiguration.pitch = hitConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayBrokenGlassSound()
        {
            AudioManager.OnSoundPlay?.Invoke(brokenGlassClipsBank, brokenGlassConfiguration, transform, 0f);
            brokenGlassConfiguration.pitch = brokenGlassConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
    }
}