using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private PauseMenu pauseMenu;

        public static UnityAction PauseMenuClosed;

        private void OnEnable()
        {
            GameManager.OnGamePaused += OpenPauseMenu;
            GameManager.OnGameResumed += ClosePauseMenu;

            pauseMenu.ResumeButtonAction += ClosePauseMenu;
        }

        private void OnDisable()
        {
            GameManager.OnGamePaused -= OpenPauseMenu;
            GameManager.OnGameResumed -= ClosePauseMenu;

            pauseMenu.ResumeButtonAction -= ClosePauseMenu;
        }

        private void OpenPauseMenu()
        {
            Time.timeScale = 0;

            pauseMenu.gameObject.SetActive(true);

            input.EnableUIInput();
        }

        private void ClosePauseMenu()
        {
            Time.timeScale = 1;

            pauseMenu.gameObject.SetActive(false);

            // TODO: manage companion case
            input.EnablePlayerInput();

            PauseMenuClosed?.Invoke();
        }
    }

}
