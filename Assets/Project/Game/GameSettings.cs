using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Range(0f, 1f)]
    public float Volume = 1.0f;
    [Range(0f, 1f)]
    public float Difficulty = 0.5f;
    [Range(.8f, 2f)]
    public float ArenaScale = 1f;

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
}
