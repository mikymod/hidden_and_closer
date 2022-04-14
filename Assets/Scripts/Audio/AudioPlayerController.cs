using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO walkStandClipsBank;
        [SerializeField] private AudioConfigurationSO walkStandConfiguration;
        [SerializeField] private AudioClipsBankSO walkCrouchClipsBank;
        [SerializeField] private AudioConfigurationSO walkCrouchConfiguration;
        [SerializeField] private AudioClipsBankSO deathClipsBank;
        [SerializeField] private AudioConfigurationSO deathConfiguration;

        public void PlayStandWalkSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(walkStandClipsBank, walkStandConfiguration, transform, 0f);
        }

        public void PlayCrouchWalkSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(walkCrouchClipsBank, walkCrouchConfiguration, transform, 0f);
        }

        public void PlayDeathSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(deathClipsBank, deathConfiguration, transform, 0f);
        }
    }
}
