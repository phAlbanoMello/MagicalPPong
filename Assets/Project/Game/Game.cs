using System;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    LivelyCamera livelyCamera;
    [SerializeField]
    bool CameraEffects;

    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle southPaddle, northPaddle;

    [SerializeField, Min(2)]
    int pointsToWin = 3;

    [SerializeField, Min(1f)]
    float newGameDelay = 3f;

    [SerializeField]
    TextMeshProUGUI countdownText;

    private float countdownUntilNewGame;
    private Vector2 arenaExtents;

    public bool RunUpdate { get; private set; }

    internal void Init(Extents extents)
    {
        arenaExtents = new Vector2(extents._width, extents._lenght);

        northPaddle.Setup(arenaExtents.y/2);
        southPaddle.Setup(-arenaExtents.y/2);

        StartCountdown();
        TogglePlayGame(true); //Starts game loop by SystemManager;
    }

    public void TogglePlayGame(bool play)
    {
        RunUpdate = play; 
    }

    void StartCountdown()
    {
        countdownUntilNewGame = newGameDelay;
    }

    void StartNewGame()
    {
        ball.StartNewGame();
        southPaddle.StartNewGame();
        northPaddle.StartNewGame();
    }

    public void UpdateGame(float deltaTime)
    {
        if (countdownUntilNewGame > 0f)
        {
            UpdateCountdown();
            return;
        }

        southPaddle.Move(ball.Position.x, arenaExtents.x/2);
        northPaddle.Move(ball.Position.x, arenaExtents.x / 2);

        UpdateBall();
    }

    void UpdateBall()
    {
        ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
        ball.UpdateVisualization();
    }

    void UpdateCountdown()
    {
        countdownUntilNewGame -= Time.deltaTime;
        if (countdownUntilNewGame <= 0f)
        {
            countdownText.gameObject.SetActive(false);
            StartNewGame();
        }
        else
        {
            float displayValue = Mathf.Ceil(countdownUntilNewGame);
            if (displayValue < newGameDelay)
            {
                countdownText.SetText("{0}", displayValue);
            }
        }
    }

    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y / 2 - ball.Extents;
        if (ball.Position.y < -yExtents)
        {
            BounceY(-yExtents, southPaddle, northPaddle);
        }
        else if (ball.Position.y > yExtents)
        {
            BounceY(yExtents, northPaddle, southPaddle);
        }
    }

    void BounceY(float boundary, Paddle defender, Paddle attacker)
    {
        float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
        float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

        BounceXIfNeeded(bounceX);
        bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

        if(CameraEffects)livelyCamera.PushXZ(ball.Velocity);

        ball.BounceY(boundary);
        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        else
        {
            if (CameraEffects) livelyCamera.JostleY();
            if (attacker.ScorePoint(pointsToWin))
            {
                EndGame();
            }
        }
    }

    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x/2 - ball.Extents;
        if (x < -xExtents) //Is hitting the left side of the arena
        {
            if (CameraEffects) livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
        }
        else if (x > xExtents)//Is hitting the right side of the arena
        {
            if (CameraEffects) livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
        }
    }

    void EndGame()
    {
        StartCountdown();
        countdownText.SetText("GAME OVER");
        countdownText.gameObject.SetActive(true);
        ball.EndGame();
    }

}
