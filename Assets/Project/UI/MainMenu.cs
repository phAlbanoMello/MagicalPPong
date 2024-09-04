using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IMenu
{
    [SerializeField]
    Button StartBtn, SettingsBtn, ExitBtn;

    [SerializeField]
    GameObject Title;

    private IMenu settingsMenu;

    public bool IsOpen { get; private set; }

    public void Setup(IMenu settingsMenu)
    {
        this.settingsMenu = settingsMenu;

        Open();
        SystemManager.Instance.SetMenu(this);
    }

    public void Close()
    {
        StartBtn.gameObject.SetActive(false);
        SettingsBtn.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(false);
        Title.SetActive(false);
        IsOpen = false;

    }
    public void Open()
    {
        StartBtn.gameObject.SetActive(true);
        SettingsBtn.gameObject.SetActive(true);
        ExitBtn.gameObject.SetActive(true);
        Title.SetActive(true);
        IsOpen = true;
    }

    public void SetCallbacks()
    {
        StartBtn.onClick.AddListener(OnStartButtonClicked);
        SettingsBtn.onClick.AddListener(OnSettingsButtonClicked);
        ExitBtn.onClick.AddListener(OnExitButtonClicked);

        SetButtonSounds(StartBtn, SettingsBtn, ExitBtn);
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

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked");
        Application.Quit();
    }

    private void OnSettingsButtonClicked()
    {
        settingsMenu.Open();
    }

    private void OnStartButtonClicked()
    {
        Close();
        SystemManager.Instance.PlayGame();
    }

    public void SaveSettings(){}

    public void ToggleInteraction(bool interact)
    {
        StartBtn.interactable = interact;
        SettingsBtn.interactable = interact;
        ExitBtn.interactable= interact;
    }

    bool IMenu.IsOpen()
    {
        return IsOpen;
    }
}
