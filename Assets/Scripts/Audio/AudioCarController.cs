using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class AudioCarController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO moveClipsBank;
        [SerializeField] private AudioConfigurationSO moveConfiguration;
        private bool isPlaying = false;

        public void PlayMoveSound()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                AudioEventsManager.OnSoundPlay?.Invoke(moveClipsBank, moveConfiguration, transform, 0f);
            }

        }

        public void StopMoveSound()
        {
            if (isPlaying)
            {
                AudioEventsManager.OnSoundStop?.Invoke(moveClipsBank, transform, 0f);
                isPlaying = false;
            }
        }
    }
}
