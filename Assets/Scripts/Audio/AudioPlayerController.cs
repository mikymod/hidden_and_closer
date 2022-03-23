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

        public void PlayStandWalkSound()
        {
            AudioManager.OnSoundPlay?.Invoke(walkStandClipsBank, walkStandConfiguration, transform, 0f);
        }
        public void PlayCrouchWalkSound()
        { 
            AudioManager.OnSoundPlay?.Invoke(walkCrouchClipsBank, walkCrouchConfiguration, transform, 0f);
        }
        public void PlayHitSound()
        {
            AudioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, transform, 0f);
        }
        public void PlayShootSound() => AudioManager.OnSoundPlay?.Invoke(shootClipsBank, shootConfiguration, transform, 0f);
        public void PlayDeathSound() => AudioManager.OnSoundPlay?.Invoke(deathClipsBank, deathConfiguration, transform, 0f);
    }
}
