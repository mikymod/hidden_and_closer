using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO walkStandClipsBank;
        [SerializeField] private AudioConfigurationSO walkStandConfiguration;
        [SerializeField] private AudioClipsBankSO walkCrouchClipsBank;
        [SerializeField] private AudioConfigurationSO walkCrouchConfiguration;
        [SerializeField] private AudioClipsBankSO shootClipsBank;
        [SerializeField] private AudioConfigurationSO shootConfiguration;
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;
        [SerializeField] private AudioClipsBankSO deathClipsBank;
        [SerializeField] private AudioConfigurationSO deathConfiguration;
        [SerializeField] private AudioManager audioManager;

        
        public void PlayStandWalkSound() => audioManager.OnSoundPlay?.Invoke(walkStandClipsBank, walkStandConfiguration, false, 0f);
        public void PlayCrouchWalkSound() => audioManager.OnSoundPlay?.Invoke(walkCrouchClipsBank, walkCrouchConfiguration, false, 0f);
        public void PlayShootSound() => audioManager.OnSoundPlay?.Invoke(shootClipsBank, shootConfiguration, false, 0f);
        public void PlayHitSound() => audioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, false, 0f);
        public void PlayDeathSound() => audioManager.OnSoundPlay?.Invoke(deathClipsBank, deathConfiguration, false, 0f);
        

    }
}
