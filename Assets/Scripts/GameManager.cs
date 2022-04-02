using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputHandler input = default;

        public static UnityAction OnGameResumed;
        public static UnityAction OnGamePaused;

        public bool GamePaused { get; private set; } = false;

        private void OnEnable()
        {
            input.pause += ScenePauseResume;
        }

        private void OnDisable()
        {
            input.pause -= ScenePauseResume;
        }

        private void ScenePauseResume()
        {
            GamePaused = !GamePaused;

            if (GamePaused)
            {
                Time.timeScale = 0;
                OnGamePaused?.Invoke();
            }
            else
            {
                Time.timeScale = 1;
                OnGameResumed?.Invoke();
            }
        }
    }
}
