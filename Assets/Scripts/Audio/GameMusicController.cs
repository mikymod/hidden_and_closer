using System.Collections.Generic;
using UnityEngine;
using HNC.Save;
using UnityEngine.Events;
using System.Collections;

namespace HNC.Audio
{
    public class GameMusicController : MonoBehaviour
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
        [SerializeField] private AudioClipsBankSO StingerSafeSpotClipsBank;
        [SerializeField] private AudioConfigurationSO genericMusicConfiguration;
        [SerializeField] private AudioConfigurationSO genericStingerConfiguration;

        public static UnityAction OnPlayCheckPointStinger;

        public float FadeInTime;
        public float FadeOutTime;

        private EnemyFSMState currentState;

        private Dictionary<GameObject, EnemyFSMState> enemiesState = new Dictionary<GameObject, EnemyFSMState>();

        private bool isInTransition;

        private void Start()
        {
            isInTransition = false;
            currentState = EnemyFSMState.Idle;
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(MusicIdleClipsBank, genericMusicConfiguration, null, 0f);
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(MusicAlertClipsBank, genericMusicConfiguration, null, 0);
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(MusicAttackClipsBank, genericMusicConfiguration, null, 0);
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(MusicSearchClipsBank, genericMusicConfiguration, null, 0);
            ChangeMusicOnState();
        }

        private void OnEnable()
        {
            NewChangeStateEvent.OnChangeState += ChangeState;
            UIManager.TransitionGameOver += FadeOutAllMusic;
            UIManager.TransitionSceneFadeOut += FadeOutAllMusic;
            PlayerController.DeadEvent += PlayDeathStinger;
            OnPlayCheckPointStinger += PlayCheckPointStinger;
        }

        private void OnDisable()
        {
            NewChangeStateEvent.OnChangeState -= ChangeState;
            UIManager.TransitionGameOver -= FadeOutAllMusic;
            UIManager.TransitionSceneFadeOut -= FadeOutAllMusic;
            PlayerController.DeadEvent -= PlayDeathStinger;
            OnPlayCheckPointStinger -= PlayCheckPointStinger;
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
            if (isInTransition)
            {
                return;
            }
            switch (currentState)
            {
                case EnemyFSMState.Idle:
                case EnemyFSMState.Death:
                    AudioEventsManager.OnSoundPlayEsclusive.Invoke(StingerAlertClipsBank, genericStingerConfiguration, null, 0f);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Search:
                    AudioEventsManager.OnSoundPlayEsclusive.Invoke(StingerSearchClipsBank, genericStingerConfiguration, null, 0f);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicSearchClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicIdleClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Alert:
                    AudioEventsManager.OnSoundPlayEsclusive.Invoke(StingerAlertClipsBank, genericStingerConfiguration, null, 0f);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeIn?.Invoke(MusicAlertClipsBank, FadeInTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Attack:
                    AudioEventsManager.OnSoundPlayEsclusive.Invoke(StingerAttackClipsBank, genericStingerConfiguration, null, 0f);
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
            isInTransition = true;
            AudioEventsManager.OnFadeOut?.Invoke(MusicIdleClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
            AudioEventsManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
        }

        private void PlayDeathStinger()
        {
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(StingerDeathClipsBank, genericStingerConfiguration, null, 0f);
        }

        private void PlayCheckPointStinger()
        {
            StartCoroutine(CheckPointCoroutine());
            FadeOutAllMusic();
            AudioEventsManager.OnSoundPlayEsclusive?.Invoke(StingerSafeSpotClipsBank, genericStingerConfiguration, null, 0f);
        }

        private IEnumerator CheckPointCoroutine()
        {
            yield return new WaitForSeconds(2);
            ChangeMusicOnState();
        }
    }
}
