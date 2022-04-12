using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class AudioZombieController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO walkClipsBank;
        [SerializeField] private AudioConfigurationSO walkConfiguration;
        [SerializeField] private AudioClipsBankSO screamClipsBank;
        [SerializeField] private AudioConfigurationSO screamConfiguration;
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;
        [SerializeField] private AudioClipsBankSO idleClipsBank;
        [SerializeField] private AudioConfigurationSO idleConfiguration;
        [SerializeField] private AudioClipsBankSO searchingClipsBank;
        [SerializeField] private AudioConfigurationSO searchingConfiguration;
        [SerializeField] private AudioClipsBankSO alertBank;
        [SerializeField] private AudioConfigurationSO alertConfiguration;

        public void PlayWalkSound()
        {
            AudioManager.OnSoundPlay?.Invoke(walkClipsBank, walkConfiguration, transform, 0f);
            walkConfiguration.pitch = walkConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayScreamSound() => AudioManager.OnSoundPlay?.Invoke(screamClipsBank, screamConfiguration, transform, 0f);
        public void PlayHitSound()
        {
            AudioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, transform, 0f);
            hitConfiguration.pitch = hitConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayIdleSound()
        {
            AudioManager.OnSoundPlay?.Invoke(idleClipsBank, idleConfiguration, transform, 0f);
            idleConfiguration.pitch = idleConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }

        public void PlaySearchingSound()
        {
            AudioManager.OnSoundPlay?.Invoke(searchingClipsBank, searchingConfiguration, transform, 0f);
            searchingConfiguration.pitch = searchingConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayAlertSound()
        {
            AudioManager.OnSoundPlay?.Invoke(alertBank, alertConfiguration, transform, 0f);
            alertConfiguration.pitch = alertConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
    }
}
