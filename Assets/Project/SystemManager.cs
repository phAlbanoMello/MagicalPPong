using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }


    [Header("Game")]
    [SerializeField]
    Game game;

    [SerializeField]
    Arena arena;

    [SerializeField]
    CameraManager cameraManager;

    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    PauseController pauseController;

    [SerializeField]
    bool autoAdjustCamera;

    private InputActionsBase baseActions;
    private MainMenu menu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Setup()
    {
        arena.Init();

        if (autoAdjustCamera) cameraManager.Setup(arena.Extents);

        soundManager.Init();
        game.Init(arena.Extents, soundManager);
        game.TogglePlayGame(false);

        pauseController.Init();
        
        baseActions = new InputActionsBase();
        baseActions.Player.Pause.started += ctx => { OpenPausePopup(); };
        baseActions.Enable();
    }

    public void PlayGame()
    {
        UpdateVolume();
        
        game.UpdateDifficulty();
        game.TogglePlayGame(true);
    }

    public void ExitGame()
    {
        game.TogglePlayGame(false);
        game.ResetGame();
        menu.Open();
    }

    private void Update()
    {
        if (game.RunUpdate)
        {
            game.UpdateGame(Time.deltaTime);
        }
    }

    private void OpenPausePopup()
    {
        if (game.RunUpdate)
        {
            Debug.Log("Opening Pause PopUp");
            pauseController.TogglePause();
        }
    }

    public void UpdateArena()
    {
        arena.UpdateScale();
        game.UpdateArenaExtents(arena.Extents);
        if (autoAdjustCamera) cameraManager.Setup(arena.Extents);
    }

    public void UpdateVolume()
    {
        soundManager.UpdateVolume();
    }

    public void SetMenu(MainMenu menu)
    {
        this.menu = menu;
    }

    public SoundManager GetSoundManager()
    {
        if (soundManager == null)
        {
            soundManager = new SoundManager();
            soundManager.Init();
            soundManager.UpdateVolume();
            return soundManager;
        }
        else
        {
            return soundManager;
        }
    }
}
