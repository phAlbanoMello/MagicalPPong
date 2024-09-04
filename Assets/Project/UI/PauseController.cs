using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private PauseModel model;
    private PauseView view;

    public void Init()
    {
        model = new PauseModel();
        view = GetComponent<PauseView>();

        view.AddContinueListener(OnContinuePressed);
        view.AddExitListener(OnQuitPressed);

        model.SetPaused(false);
        view.HidePopup();
    }

    public void TogglePause()
    {
        if (model.IsPaused)
        {
            model.SetPaused(false);
            view.HidePopup();
        }
        else
        {
            model.SetPaused(true);
            view.ShowPopup();
        }
    }

    private void OnContinuePressed()
    {
        TogglePause();
    }
    private void OnQuitPressed()
    {
        SystemManager.Instance.ExitGame();
        view.HidePopup();
    }
}
