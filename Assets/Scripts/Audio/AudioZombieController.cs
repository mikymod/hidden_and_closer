using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
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
        [SerializeField] private AudioManager audioManager;

        
        public void PlayWalkSound() => audioManager.OnSoundPlay?.Invoke(walkClipsBank, walkConfiguration, false, 0f);
        public void PlayScreamSound() => audioManager.OnSoundPlay?.Invoke(screamClipsBank, screamConfiguration, false, 0f);
        public void PlayHitSound() => audioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, false, 0f);
        public void PlayIdleSound() => audioManager.OnSoundPlay?.Invoke(idleClipsBank, idleConfiguration, false, 0f);
        public void PlaySearchingSound() => audioManager.OnSoundPlay?.Invoke(searchingClipsBank, searchingConfiguration, false, 0f);
        public void PlayAlertSound() => audioManager.OnSoundPlay?.Invoke(alertBank, alertConfiguration, false, 0f);
        

    }
}
