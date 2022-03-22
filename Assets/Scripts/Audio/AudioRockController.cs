using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioRockController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;
        [SerializeField] private AudioManager audioManager;

        public void PlayHitSound() => audioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, false, 0f);
    }
}
