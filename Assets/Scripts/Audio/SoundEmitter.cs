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
        private Transform originalParent;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            originalParent = transform.parent.transform;
        }

        public void Play(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, Transform transform, float fadeTime)
        {
            audioSource.transform.parent = transform;
            audioSource.transform.localPosition = Vector3.zero;
            audioSource.clip = audioClipBank.GetClip();
            audioConfig.ApplyTo(audioSource);
            audioSource.Play();
            if (fadeTime > 0)
            {
                StartCoroutine(FadeIn(fadeTime));
            }
            if (!audioSource.loop)
            {
                float clipLengthRemaining = audioSource.clip.length - audioSource.time;
                StartCoroutine(AudioClipFinishPlaying(transform, clipLengthRemaining));
            }
        }

        public void Resume() => audioSource.UnPause();
        public void Pause() => audioSource.Pause();
        public void Stop(Transform transform, float lenght)
         {
            audioSource.transform.parent = originalParent;
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        } 
        private IEnumerator AudioClipFinishPlaying(Transform transform, float lenght)
        {
            yield return new WaitForSeconds(lenght);
            Stop(transform, lenght);
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
