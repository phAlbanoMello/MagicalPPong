using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private MainMenu mainMenu;
    private SystemManager systemManager;

    private async void Awake()
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

        await InitializeScenes();

        SystemManager.Instance.Setup();
    }

    private async Task InitializeScenes()
    {
        await LoadSceneAsync("Game");
        await LoadSceneAsync("MainMenu");
    }

    public async Task<bool> LoadSceneAsync(string name)
    {
        if (!SceneManager.GetSceneByName(name).isLoaded)
        {
            var tcs = new TaskCompletionSource<bool>();
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                if (scene.name == name)
                {
                    tcs.SetResult(true);
                }
            };

            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

            await tcs.Task;
            return true;
        }
        else
        {
            Debug.LogWarning(name + " scene is already loaded.");
            return false;
        }
    }

    public void UnloadScene(string name)
    {
        if (SceneManager.GetSceneByName(name).isLoaded)
        {
            SceneManager.UnloadSceneAsync(name);
        }
        else
        {
            Debug.LogWarning(name + " scene is not loaded.");
        }
    }

    public static T FindObjectOfTypeInScene<T>(string sceneName) where T : UnityEngine.Object
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Scene {sceneName} is not loaded.");
            return null;
        }

        foreach (GameObject rootGameObject in scene.GetRootGameObjects())
        {
            T component = rootGameObject.GetComponentInChildren<T>();
            if (component != null)
            {
                return component;
            }
        }

        Debug.LogWarning($"No object of type {typeof(T)} found in scene {sceneName}.");
        return null;
    }
}
