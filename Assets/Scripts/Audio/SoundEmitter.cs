using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class SoundEmitter : MonoBehaviour
    {
        private int id;
        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClipsBank audioClipBank, AudioConfiguration audioConfig)
        {
            audioSource.clip = audioClipBank.GetClip();
            audioConfig.ApplyTo(audioSource);
            audioSource.Play();
            if (!audioSource.loop)
            {
                StartCoroutine(AudioClipFinishPlaying(audioClipBank.GetClip().length, audioSource));
            }
        }

        public void Resume() => audioSource.UnPause();
        public void Pause() => audioSource.Pause();
        public void Stop() => audioSource.Stop();
        public void Finish()
        {
            if (!audioSource.loop) return;

            audioSource.loop = false;
            float clipLenghtRemaining = audioSource.clip.length - audioSource.time;
            
            //CheckThis, it works?
            StartCoroutine(AudioClipFinishPlaying(clipLenghtRemaining ,audioSource));
        }
        private IEnumerator AudioClipFinishPlaying(float lenght, AudioSource source)
        {
            yield return new WaitForSeconds(lenght);
            source.gameObject.SetActive(false);
        }

        public IEnumerator FadeIn(float fadeTime)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 1f;
        }

        public IEnumerator FadeOut(float fadeTime)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.gameObject.SetActive(false);
        }

        public void SetUniqueID() => id = Guid.NewGuid().GetHashCode();
        public int GetUniqueID() => id;
        public bool CheckForUniqueID(int key) => key == id;


    }
}
