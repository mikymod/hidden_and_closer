using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private StartMenu startMenu;
        [SerializeField] private SettingsMenu settingsMenu;

        [SerializeField] private InputHandler input;
        [SerializeField] private SaveSystem saveSystem;

        [SerializeField] private string firstScene;

        private bool saveDataExists;

        private IEnumerator Start()
        {
            input.EnableUIInput();

            yield return new WaitForSeconds(0.4f); // Waiting time for all scenes to be loaded

            SetMenuScreen();
        }

        private void SetMenuScreen()
        {
            saveDataExists = saveSystem.LoadGameDataFromDisk();

            startMenu.SetMenuScreen(saveDataExists);
            startMenu.NewGameButtonAction += NewGameButtonPressed;
            startMenu.ContinueButtonAction += ContinueButtonPressed;
            startMenu.SettingsButtonAction += SettingsButtonPressed;
            startMenu.QuitButtonAction += QuitButtonPressed;
        }

        private void NewGameButtonPressed()
        {
            // Avoid multiple button click
            EventSystem.current.SetSelectedGameObject(null);

            saveSystem.CreateNewGameFile();

            SceneManager.LoadScene(firstScene);
        }

        private void ContinueButtonPressed()
        {
            SceneManager.LoadScene(saveSystem.SaveData.Player.Scene);
        }

        private void SettingsButtonPressed()
        {
            OpenSettingsMenu();
        }

        private void OpenSettingsMenu()
        {
            startMenu.gameObject.SetActive(false);

            settingsMenu.SettingsClosed += CloseSettingsMenu;
            settingsMenu.gameObject.SetActive(true);
            settingsMenu.InitSettingsMenu();
        }

        private void CloseSettingsMenu()
        {
            settingsMenu.SettingsClosed -= CloseSettingsMenu;
            settingsMenu.gameObject.SetActive(false);

            startMenu.gameObject.SetActive(true);
            startMenu.SetMenuScreen(saveDataExists);
        }

        private void QuitButtonPressed()
        {
            Application.Quit();
        }
    }
}
