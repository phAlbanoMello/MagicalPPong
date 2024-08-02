using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
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
    bool autoAdjustCamera;

    private void Awake()
    {
        arena.Init();
        if (autoAdjustCamera) cameraManager.Setup(arena.Extents);
        soundManager.Init();
        game.Init(arena.Extents, soundManager);
    }

    private void Update()
    {
        if (game.RunUpdate)
        {
            game.UpdateGame(Time.deltaTime);
        }
    }
}
