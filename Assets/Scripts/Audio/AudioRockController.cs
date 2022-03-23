using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioRockController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;

        public void PlayHitSound()
        {
            AudioManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, transform, 0f);
            hitConfiguration.pitch = hitConfiguration.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
        }
    }
}
