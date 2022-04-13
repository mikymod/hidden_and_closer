using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class AudioZombieController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO walkClipsBank;
        [SerializeField] private AudioClipsBankSO runClipsBank;
        [SerializeField] private AudioClipsBankSO alertBank;
        [SerializeField] private AudioClipsBankSO attackBank;
        [SerializeField] private AudioClipsBankSO searchBank;
        [SerializeField] private AudioClipsBankSO carAttackBank;
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioClipsBankSO rockHitClipsBank;
        [SerializeField] private AudioClipsBankSO idleClipsBank;
        [SerializeField] private AudioClipsBankSO screamClipsBank;
        [SerializeField] private AudioConfigurationSO genericConfiguration;

        public void PlayWalkSound()
        {
            AudioManager.OnSoundPlay?.Invoke(walkClipsBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }

        public void PlayRunSound()
        {
            AudioManager.OnSoundPlay?.Invoke(runClipsBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayAttackSound()
        {
            AudioManager.OnSoundPlay?.Invoke(attackBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }

        public void PlayScreamSound() => AudioManager.OnSoundPlay?.Invoke(screamClipsBank, genericConfiguration, transform, 0f);
        public void PlayHitSound()
        {
            AudioManager.OnSoundPlay?.Invoke(hitClipsBank, genericConfiguration, transform, 0f);
            AudioManager.OnSoundPlay?.Invoke(rockHitClipsBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayIdleSound()
        {
            AudioManager.OnSoundPlay?.Invoke(idleClipsBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
        public void PlayAlertSound()
        {
            AudioManager.OnSoundPlay?.Invoke(alertBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }

        public void PlaySearchSound()
        {
            AudioManager.OnSoundPlay?.Invoke(searchBank, genericConfiguration, transform, 0f);
            genericConfiguration.pitch = genericConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
    }
}
