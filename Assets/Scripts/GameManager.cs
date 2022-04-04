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

            UIManager.PauseMenuClosed += SceneResume;
        }

        private void OnDisable()
        {
            input.pause -= ScenePauseResume;

            UIManager.PauseMenuClosed -= SceneResume;
        }

        private void ScenePauseResume()
        {
            GamePaused = !GamePaused;

            if (GamePaused)
            {
                OnGamePaused?.Invoke();
            }
            else
            {
                OnGameResumed?.Invoke();
            }
        }

        private void SceneResume()
        {
            GamePaused = false;
        }
    }
}
