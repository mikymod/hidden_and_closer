using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    [CreateAssetMenu(fileName = "AudioClipsBank", menuName = "HNC/AudioClipsBank")]
    public class AudioClipsBankSO : ScriptableObject
    {
       
        public AudioClipsGroup audioClipsGroups = default;

        public AudioClip GetClip()
        {
            return audioClipsGroups.NextClip();
        }

        [Serializable]
        public class AudioClipsGroup
        {

            public enum AudioSequenceMode{ Sequential, Random }
            public AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
            public AudioClip[] audioClips;
            private int currentIndex = -1;
            private int previousIndex = -1;

            public AudioClip NextClip()
            {
                if (audioClips.Length == 1) return audioClips[0];

                switch (sequenceMode)
                {
                    case AudioSequenceMode.Sequential:
                        currentIndex = (int)Mathf.Repeat(++currentIndex, audioClips.Length);
                        break;
                    case AudioSequenceMode.Random:
                        do
                        {
                            currentIndex = UnityEngine.Random.Range(0, audioClips.Length);
                        }
                        while (currentIndex == previousIndex);
                        break;
                }

                previousIndex = currentIndex;

                return audioClips[currentIndex];
            }
        }
    }
}
