using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField]
    Slider volumeSlider, difficultySlider, arenaScaleSlider;

    [SerializeField]
    Button backBtn;

    private float currentVolume = 1f;
    private float currentDifficulty = 0.5f;
    private float currentScale = 1f;

    private const string VOLUME_CHECK_SFX = "Hit";

    private Coroutine volumeSliderCoroutine;

    private IMenu previousMenu;
    public bool IsOpen { get; private set; }

    public void Setup(IMenu linkedMenu)
    {
        previousMenu = linkedMenu;
  
        Close();
    }

    public void Close()
    {
        SaveSettings();
        volumeSlider.transform.parent.gameObject.SetActive(false);
        difficultySlider.transform.parent.gameObject.SetActive(false);
        arenaScaleSlider.transform.parent.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);

        previousMenu.ToggleInteraction(true);
    }
    public void Open()
    {
        volumeSlider.transform.parent.gameObject.SetActive(true);
        difficultySlider.transform.parent.gameObject.SetActive(true);
        arenaScaleSlider.transform.parent.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(true);

        previousMenu.ToggleInteraction(false);
    }

    public void SetCallbacks()
    {
        LoadDataFromSettings();
        volumeSlider.onValueChanged.AddListener(OnVolumeValueChanged);
        difficultySlider.onValueChanged.AddListener(OnDifficultyValueChanged);
        arenaScaleSlider.onValueChanged.AddListener(OnArenaScaleValueChanged);
        backBtn.onClick.AddListener(OnBackButtonClicked);

        SoundManager sm = SystemManager.Instance.GetSoundManager();
        sm.AddButtonSoundOnEvent(backBtn, "Select", UnityEngine.EventSystems.EventTriggerType.PointerEnter);
        sm.AddButtonSoundOnEvent(backBtn, "Confirm", UnityEngine.EventSystems.EventTriggerType.PointerClick);
    }

    private void OnBackButtonClicked()
    {
        Close();
    }

    private void LoadDataFromSettings()
    {
        currentVolume = GameSettings.Instance.Volume;
        currentDifficulty = GameSettings.Instance.Difficulty;
        currentScale = GameSettings.Instance.ArenaScale;

        volumeSlider.value = currentVolume;
        difficultySlider.value = currentDifficulty;
        arenaScaleSlider.value = currentScale;
    }

    private void OnDifficultyValueChanged(float value)
    {
        currentDifficulty = value;
    }

    private void OnVolumeValueChanged(float value)
    {
        currentVolume = value;
        StartVolumeSliderCoroutine(volumeSlider);
    }

    private static void PlaySFXFeedback()
    {
        SoundManager sM = SystemManager.Instance.GetSoundManager();
        sM.PlaySound(VOLUME_CHECK_SFX);
    }

    private void OnArenaScaleValueChanged(float value)
    {
        currentScale = value;
        SaveSettings();
    }

    public void SaveSettings()
    {
        GameSettings.Instance.Difficulty = currentDifficulty;
        GameSettings.Instance.Volume = currentVolume;
        GameSettings.Instance.ArenaScale = currentScale;
        SystemManager.Instance.UpdateVolume();
        SystemManager.Instance.UpdateArena();
    }

    public void ToggleInteraction(bool interact)
    {
        volumeSlider.interactable = interact;
        difficultySlider.interactable = interact;
        arenaScaleSlider.interactable = interact;
        backBtn.interactable = interact;
    }

    bool IMenu.IsOpen()
    {
        return IsOpen;
    }

    private void StartVolumeSliderCoroutine(Slider slider)
    {
        if (volumeSliderCoroutine != null)
        {
            StopCoroutine(volumeSliderCoroutine);
        }
        volumeSliderCoroutine = StartCoroutine(DetectVolumeSliderEnd(slider));
    }

    private IEnumerator DetectVolumeSliderEnd(Slider slider)
    {
        float waitTime = 0.2f;
        yield return new WaitForSeconds(waitTime);

        float lastValue = currentVolume;
        if (Mathf.Abs(lastValue - slider.value) < .2)
        {
            yield return null;
        }

        OnVolumeSliderInteractionEnded(slider);
    }

    private void OnVolumeSliderInteractionEnded(Slider slider)
    {
        SaveSettings();
        PlaySFXFeedback();
    }
}
