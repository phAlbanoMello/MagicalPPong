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
    bool autoAdjustCamera;

    private void Awake()
    {
        arena.Init();

        if (autoAdjustCamera ) cameraManager.Init(arena.Extents);

        game.Init(arena.Extents);

    }

    private void Update()
    {
        if (game.RunUpdate)
        {
            game.UpdateGame(Time.deltaTime);
        }
    }
}
