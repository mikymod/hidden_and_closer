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
        [SerializeField] private AudioManager audioManager;

        public void PlayBuzzingSound() => audioManager.OnSoundPlay?.Invoke(buzzingClipsBank, buzzingConfiguration, true, 0f);
        public void StopBuzzingSound() => audioManager.OnSoundStop?.Invoke(0f);
        public void PlayHitSound() => audioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, false, 0f);
        public void PlayBrokenGlassSound() => audioManager.OnSoundPlay?.Invoke(brokenGlassClipsBank, brokenGlassConfiguration, false, 0f);
        
    }
}
