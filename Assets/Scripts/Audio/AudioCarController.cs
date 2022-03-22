using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class AudioCarController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO moveClipsBank;
        [SerializeField] private AudioConfigurationSO moveConfiguration;
        [SerializeField] private AudioManager audioManager;

        public void PlayMoveSound() => audioManager.OnSoundPlay?.Invoke(moveClipsBank, moveConfiguration, true, 0f);
    }
}
