using System.Collections;
using UnityEngine;

namespace HNC
{
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private Transform originalParent;
        public LayerMask obstacleMask;

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
                audioSource.transform.localPosition = Vector3.zero + Vector3.up;
            }
            else
            {
                audioSource.transform.localPosition = Vector3.zero;
            }

            audioSource.clip = audioClipBank.GetClip();
            audioConfig.ApplyTo(audioSource);
            audioConfig.pitch = audioConfig.randomPitch ? Random.Range(0.9f, 1.2f) : 1;
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
            if (audioConfig.lpFilterOn)
            {
                if (CheckForPlayerVision())
                {
                    audioSource.GetComponent<AudioLowPassFilter>().enabled = true;
                }
                else
                {
                    audioSource.GetComponent<AudioLowPassFilter>().enabled = false;
                }
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

        private bool CheckForPlayerVision()
        {
            float distanceToTarget = Vector3.Distance(transform.position, GameObject.Find("Player(Clone)").transform.position);
            if (distanceToTarget <= 16)
            {
                Vector3 dirToTarget = (GameObject.Find("Player(Clone)").transform.position - transform.position).normalized;
                Debug.DrawRay(transform.position, dirToTarget * 16);
                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
