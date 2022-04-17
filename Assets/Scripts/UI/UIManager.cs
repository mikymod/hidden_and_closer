using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private SaveSystem saveSystem;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject sceneTransitionUI;

        public static UnityAction TransitionGameOver;
        public static UnityAction TransitionSceneFadeOut;

        private void Awake()
        {
            gameUI.SetActive(true);
            sceneTransitionUI.SetActive(true);
        }

        private void OnEnable()
        {
            TransitionGameOver += OnTransitionGameOver;
            TransitionSceneFadeOut += OnTransitionSceneFadeOut;

            input.pause += OpenPauseMenu;
        }

        private void OnDisable()
        {
            TransitionGameOver -= OnTransitionGameOver;
            TransitionSceneFadeOut -= OnTransitionSceneFadeOut;

            input.pause -= OpenPauseMenu;
        }

        private void OnTransitionSceneFadeOut()
        {
            sceneTransitionUI.SetActive(true);
        }

        private void OnTransitionGameOver() => StartCoroutine(TransitionGameOverCoroutine());

        private IEnumerator TransitionGameOverCoroutine()
        {
            yield return new WaitForSeconds(2);

            gameOverUI.SetActive(true);

            yield return new WaitForSeconds(2);

            SceneManager.LoadScene(saveSystem.SaveData.Player.Scene);
        }

        private void OpenPauseMenu()
        {
            input.pause -= OpenPauseMenu;

            input.pause += ClosePauseMenu;
            pauseMenu.ResumeButtonAction += ResumeButtonPressed;
            pauseMenu.SettingsButtonAction += SettingsButtonPressed;
            pauseMenu.ReturnButtonAction += ReturnButtonPressed;
            pauseMenu.QuitButtonAction += QuitButtonPressed;

            Time.timeScale = 0;

            pauseMenu.gameObject.SetActive(true);
            pauseMenu.SetMenuScreen();

            input.EnableUIInput();
        }

        private void ClosePauseMenu()
        {
            input.pause -= ClosePauseMenu;
            pauseMenu.ResumeButtonAction -= ResumeButtonPressed;
            pauseMenu.SettingsButtonAction -= SettingsButtonPressed;
            pauseMenu.ReturnButtonAction -= ReturnButtonPressed;
            pauseMenu.QuitButtonAction -= QuitButtonPressed;

            input.pause += OpenPauseMenu;

            Time.timeScale = 1;

            pauseMenu.gameObject.SetActive(false);

            // TODO: manage companion case
            input.EnablePlayerInput();
        }

        private void ResumeButtonPressed() => ClosePauseMenu();

        private void SettingsButtonPressed() => OpenSettingsMenu();

        private void ReturnButtonPressed()
        {
            ClosePauseMenu();
            input.DisableAllInput();
            SceneManager.LoadScene(0);
        }

        private void QuitButtonPressed()
        {
            Application.Quit(0);
        }

        private void OpenSettingsMenu()
        {
            pauseMenu.gameObject.SetActive(false);

            input.pause -= ClosePauseMenu;
            input.pause += CloseSettingsMenu;
            settingsMenu.SettingsClosed += CloseSettingsMenu;

            settingsMenu.gameObject.SetActive(true);
            settingsMenu.InitSettingsMenu();
        }

        private void CloseSettingsMenu()
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.SetMenuScreen();

            input.pause -= CloseSettingsMenu;
            settingsMenu.SettingsClosed -= CloseSettingsMenu;
            input.pause += ClosePauseMenu;


            settingsMenu.gameObject.SetActive(false);
        }
    }
}
