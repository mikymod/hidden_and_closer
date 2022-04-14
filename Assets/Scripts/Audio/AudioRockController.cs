using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class AudioRockController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioConfigurationSO hitConfiguration;

        public void PlayHitSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(hitClipsBank, hitConfiguration, transform, 0f);
        }
    }
}
