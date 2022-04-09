using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private GameObject gameUI;

        private void Awake()
        {
            gameUI.SetActive(true);
        }

        private void OnEnable()
        {
            input.pause += OpenPauseMenu;
        }

        private void OnDisable()
        {
            input.pause -= OpenPauseMenu;
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
            pauseMenu.InitPauseMenu();

            input.pause -= CloseSettingsMenu;
            settingsMenu.SettingsClosed -= CloseSettingsMenu;
            input.pause += ClosePauseMenu;

            settingsMenu.gameObject.SetActive(false);
        }
    }
}
