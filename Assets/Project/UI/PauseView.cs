using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;

    public void Setup()
    {
        SetButtonSounds(continueButton, exitButton);
    }

    public void ShowPopup()
    {
        pausePopup.SetActive(true);
    }

    public void HidePopup()
    {
        pausePopup.SetActive(false);
    }

    public void AddContinueListener(UnityEngine.Events.UnityAction action)
    {
        if (action != null)
        {
            continueButton.onClick.AddListener(action);
        }
    }

    public void AddExitListener(UnityEngine.Events.UnityAction action)
    {
        if (action != null)
        {
            exitButton.onClick.AddListener(action);
        }
    }

    private void SetButtonSounds(params Button[] buttons)
    {
        SoundManager soundManager = SystemManager.Instance.GetSoundManager();

        for (int i = 0; i < buttons.Length; i++)
        {
            soundManager.AddButtonSoundOnEvent(buttons[i], "Select", UnityEngine.EventSystems.EventTriggerType.PointerEnter);
            soundManager.AddButtonSoundOnEvent(buttons[i], "Confirm", UnityEngine.EventSystems.EventTriggerType.PointerClick);
        }
    }
}
