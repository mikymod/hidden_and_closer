using System.Collections.Generic;
using UnityEngine;

namespace HNC.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO MusicIdleClipsBank;
        [SerializeField] private AudioClipsBankSO MusicAlertClipsBank;
        [SerializeField] private AudioClipsBankSO MusicAttackClipsBank;
        [SerializeField] private AudioClipsBankSO MusicSearchClipsBank;
        [SerializeField] private AudioClipsBankSO StingerIdleClipsBank;
        [SerializeField] private AudioClipsBankSO StingerAlertClipsBank;
        [SerializeField] private AudioClipsBankSO StingerAttackClipsBank;
        [SerializeField] private AudioClipsBankSO StingerSearchClipsBank;
        [SerializeField] private AudioClipsBankSO StingerDeathClipsBank;
        [SerializeField] private AudioConfigurationSO genericMusicConfiguration;
        [SerializeField] private AudioConfigurationSO genericStingerConfiguration;

        public float FadeInTime;
        public float FadeOutTime;

        private EnemyFSMState currentState;

        private Dictionary<GameObject, EnemyFSMState> enemiesState = new Dictionary<GameObject, EnemyFSMState>();

        private void Start()
        {
            currentState = EnemyFSMState.Idle;
            AudioEventsManager.OnSoundPlay?.Invoke(MusicIdleClipsBank, genericMusicConfiguration, null, 0f);
            AudioEventsManager.OnSoundPlay?.Invoke(MusicAlertClipsBank, genericMusicConfiguration, null, 0);
            AudioEventsManager.OnSoundPlay?.Invoke(MusicAttackClipsBank, genericMusicConfiguration, null, 0);
            AudioEventsManager.OnSoundPlay?.Invoke(MusicSearchClipsBank, genericMusicConfiguration, null, 0);
            ChangeMusicOnState();
        }

        private void OnEnable()
        {
            NewChangeStateEvent.OnChangeState += ChangeState;
            UIManager.TransitionGameOver += FadeOutAllMusic;
            UIManager.TransitionSceneFadeOut += FadeOutAllMusic;
            PlayerController.DeadEvent += PlayDeathSound;
        }

        private void OnDisable()
        {
            NewChangeStateEvent.OnChangeState -= ChangeState;
            UIManager.TransitionGameOver -= FadeOutAllMusic;
            UIManager.TransitionSceneFadeOut -= FadeOutAllMusic;
            PlayerController.DeadEvent -= PlayDeathSound;
        }

        private void ChangeState(GameObject enemy, EnemyFSMState state)
        {
            if (!enemiesState.ContainsKey(enemy))
            {
                enemiesState.Add(enemy, state);
            }
            else
            {
                enemiesState[enemy] = state;
            }
            currentState = EnemyFSMState.Idle;
            for (int i = (int)EnemyFSMState.Death; i >= 0; i--)
            {
                if (enemiesState.ContainsValue((EnemyFSMState)i))
                {
                    currentState = state;
                    break;
                }
            }
            ChangeMusicOnState();
        }

        private void ChangeMusicOnState()
        {
            switch (currentState)
            {
                case EnemyFSMState.Idle:
                case EnemyFSMState.Death:
                    AudioEventsManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Search:
                    AudioEventsManager.OnFadeIn?.Invoke(MusicSearchClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicIdleClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Alert:
                    AudioEventsManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicAlertClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Attack:
                    AudioEventsManager.OnFadeIn?.Invoke(MusicSearchClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicAlertClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicAttackClipsBank, FadeInTime);
                    break;
                default:
                    break;
            }
        }
    
        private void FadeOutAllMusic()
        {
            AudioEventsManager.OnFadeOut?.Invoke(MusicIdleClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
        }

        private void PlayDeathSound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(StingerDeathClipsBank, genericStingerConfiguration, null, 0f);
        }
    }
}
