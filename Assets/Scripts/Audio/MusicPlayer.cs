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
        [SerializeField] private AudioConfigurationSO genericMusicConfiguration;
        [SerializeField] private AudioConfigurationSO genericStingerConfiguration;
        [SerializeField] private AudioConfigurationSO otherLayersMusicConfiguration;

        public float FadeInTime;
        public float FadeOutTime;

        private EnemyFSMState currentState;

        private Dictionary<GameObject, EnemyFSMState> enemiesState = new Dictionary<GameObject, EnemyFSMState>();

        private void Start()
        {
            currentState = EnemyFSMState.Idle;
            AudioManager.OnSoundPlayEsclusive?.Invoke(MusicIdleClipsBank, genericMusicConfiguration, null, 3);
            AudioManager.OnSoundPlayEsclusive?.Invoke(MusicAlertClipsBank, otherLayersMusicConfiguration, null, 0);
            AudioManager.OnSoundPlayEsclusive?.Invoke(MusicAttackClipsBank, otherLayersMusicConfiguration, null, 0);
            AudioManager.OnSoundPlayEsclusive?.Invoke(MusicSearchClipsBank, otherLayersMusicConfiguration, null, 0);
        }

        private void OnEnable()
        {
            NewChangeStateEvent.OnChangeState += ChangeState;
        }

        private void OnDisable()
        {
            NewChangeStateEvent.OnChangeState -= ChangeState;
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
                    AudioManager.OnSoundPlayEsclusive?.Invoke(StingerIdleClipsBank, genericStingerConfiguration, null, 0f);
                    AudioManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    AudioManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Search:
                    AudioManager.OnSoundPlayEsclusive?.Invoke(StingerSearchClipsBank, genericStingerConfiguration, null, 0f);
                    AudioManager.OnFadeIn?.Invoke(MusicSearchClipsBank, FadeInTime);
                    AudioManager.OnFadeOut?.Invoke(MusicIdleClipsBank, FadeOutTime);
                    AudioManager.OnFadeOut?.Invoke(MusicAlertClipsBank, FadeOutTime);
                    AudioManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Alert:
                    AudioManager.OnSoundPlayEsclusive?.Invoke(StingerAlertClipsBank, genericStingerConfiguration, null, 0f);
                    AudioManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioManager.OnFadeIn?.Invoke(MusicAlertClipsBank, FadeInTime);
                    AudioManager.OnFadeOut?.Invoke(MusicAttackClipsBank, FadeOutTime);
                    AudioManager.OnFadeOut?.Invoke(MusicSearchClipsBank, FadeOutTime);
                    break;
                case EnemyFSMState.Attack:
                    AudioManager.OnSoundPlayEsclusive?.Invoke(StingerAttackClipsBank, genericStingerConfiguration, null, 0f);
                    AudioManager.OnFadeIn?.Invoke(MusicSearchClipsBank, FadeInTime);
                    AudioManager.OnFadeIn?.Invoke(MusicIdleClipsBank, FadeInTime);
                    AudioManager.OnFadeIn?.Invoke(MusicAlertClipsBank, FadeInTime);
                    AudioManager.OnFadeIn?.Invoke(MusicAttackClipsBank, FadeInTime);
                    break;
            }
        }
    }
}
