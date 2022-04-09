using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNC
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private InputHandler input = default;
        [SerializeField] private Button continueButton = default;
        [SerializeField] private Button newGameButton = default;
        [SerializeField] private Button settingsButton = default;
        [SerializeField] private Button quitButton = default;

        public UnityAction ContinueButtonAction;
        public UnityAction NewGameButtonAction;
        public UnityAction SettingsButtonAction;
        public UnityAction QuitButtonAction;

        public void SetMenuScreen(bool hasSaveData)
        {
            continueButton.interactable = hasSaveData;
            if (hasSaveData)
            {
                continueButton.Select();
            }
            else
            {
                newGameButton.Select();
            }
        }

        public void ContinueButtonPressed() => ContinueButtonAction?.Invoke();
        public void NewGameButtonPressed() => NewGameButtonAction?.Invoke();
        public void SettingsButtonPressed() => SettingsButtonAction?.Invoke();
        public void QuitButtonPressed() => QuitButtonAction?.Invoke();
    }
}
