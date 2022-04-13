using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private Transform originalParent;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            originalParent = transform.parent.transform;
        }

        public void Play(AudioClipsBankSO audioClipBank, AudioConfigurationSO audioConfig, Transform transform, float fadeTime)
        {
            if (transform != null)
            {
                audioSource.transform.parent = transform;
            }
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
                StartCoroutine(AudioClipFinishPlaying(transform, clipLengthRemaining, fadeTime));
            }
        }
        
        public void Resume(float fadeTime) 
        {
            if (fadeTime > 0)
            {
                StartCoroutine(FadeInForResume(fadeTime));
            }
            else
            {

                audioSource.UnPause();
            }
        }
        
        public void Pause(float fadeTime)
        {
            if (fadeTime > 0)
            {
                StartCoroutine(FadeOutForPause(fadeTime));
            }
            else
            { 
                audioSource.Pause();
            }
        }
        
        public void Stop(Transform transform, float fadeTime)
         {
            if (fadeTime <= 0)
            {
                audioSource.volume = 0f;
                audioSource.transform.parent = originalParent;
                StopAllCoroutines();
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(FadeOut(fadeTime));
            }
        } 
        
        private IEnumerator AudioClipFinishPlaying(Transform transform, float lenght, float fadeTime)
        {
            yield return new WaitForSeconds(lenght);
            Stop(transform, fadeTime);
        }

        private IEnumerator FadeIn(float fadeTime)
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

        private IEnumerator FadeOut(float fadeTime)
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
            audioSource.transform.parent = originalParent;
            StopAllCoroutines();
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        }

        public IEnumerator FadeInForResume(float fadeTime)
        {
            audioSource.UnPause();
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

        public IEnumerator FadeOutForPause(float fadeTime)
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
            audioSource.Pause();
        }

        public IEnumerator FadeOutWithoutStop(float fadeTime)
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
        }
    
        public void CallFadeIn(float fadeTime)
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn(fadeTime));
        }

        public void CallFadeOutWithoutStop(float fadeTime)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutWithoutStop(fadeTime));
        }
    }
}
