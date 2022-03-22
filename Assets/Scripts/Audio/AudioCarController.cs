using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioCarController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO moveClipsBank;
        [SerializeField] private AudioConfigurationSO moveConfiguration;

        public void PlayMoveSound()
        {
            AudioManager.OnSoundPlay?.Invoke(moveClipsBank, moveConfiguration, true, 0f);
            AudioManager.OnDoingNoise?.Invoke(transform);
        }
    }
}
