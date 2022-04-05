using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton = default;
    [SerializeField] private Button settingsButton = default;
    [SerializeField] private Button returnButton = default;
    [SerializeField] private Button quitButton = default;
    [SerializeField] private GameObject firstElement; // Required by EventSystem

    public UnityAction ResumeButtonAction;
    public UnityAction SettingsButtonAction;
    public UnityAction ReturnButtonAction;
    public UnityAction QuitButtonAction;

    public void InitPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(firstElement);
    }

    public void SetMenuScreen()
    {
        resumeButton.Select();
    }

    public void ResumeButton()
    {
        ResumeButtonAction?.Invoke();
    }

    public void SettingsPressed()
    {
        SettingsButtonAction?.Invoke();
    }

    public void ReturnPressed()
    {
        ReturnButtonAction?.Invoke();
    }

    public void QuitButton()
    {
        QuitButtonAction?.Invoke();
    }
}

