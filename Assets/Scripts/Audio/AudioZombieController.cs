using UnityEngine;

namespace HNC.Audio
{
    public class AudioZombieController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO walkClipsBank;
        [SerializeField] private AudioClipsBankSO runClipsBank;
        [SerializeField] private AudioClipsBankSO alertBank;
        [SerializeField] private AudioClipsBankSO attackBank;
        [SerializeField] private AudioClipsBankSO searchBank;
        [SerializeField] private AudioClipsBankSO carAttackBank;
        [SerializeField] private AudioClipsBankSO hitClipsBank;
        [SerializeField] private AudioClipsBankSO rockHitClipsBank;
        [SerializeField] private AudioClipsBankSO idleClipsBank;
        [SerializeField] private AudioClipsBankSO screamClipsBank;
        [SerializeField] private AudioClipsBankSO carAttackClipsBank;
        [SerializeField] private AudioClipsBankSO fallClipsBank;
        [SerializeField] private AudioConfigurationSO genericConfiguration;
        [SerializeField] private AudioConfigurationSO walkRunConfiguration;

        public void PlayWalkSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(walkClipsBank, walkRunConfiguration, transform, 0f);
        }

        public void PlayRunSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(runClipsBank, walkRunConfiguration, transform, 0f);
        }

        public void PlayAttackSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(attackBank, genericConfiguration, transform, 0f);
        }

        public void PlayCarAttackSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(carAttackClipsBank, genericConfiguration, transform, 0f);
        }

        public void PlayScreamSound() => AudioEventsManager.OnSoundPlay?.Invoke(screamClipsBank, genericConfiguration, transform, 0f);
        public void PlayHitSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(rockHitClipsBank, genericConfiguration, transform, 0f);
            AudioEventsManager.OnSoundPlay?.Invoke(hitClipsBank, genericConfiguration, transform, 0f);
        }
        public void PlayIdleSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(idleClipsBank, genericConfiguration, transform, 0f);
        }
        public void PlayAlertSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(alertBank, genericConfiguration, transform, 0f);
        }

        public void PlaySearchSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(searchBank, genericConfiguration, transform, 0f);
        }

        public void PlayFallSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(fallClipsBank, genericConfiguration, transform, 0f);
        }
    }
}
