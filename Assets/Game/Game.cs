using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    LivelyCamera livelyCamera;

    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle southPaddle, northPaddle;
    
    [SerializeField, Min(0f)]
    Vector2 arenaExtents = new Vector2(10f, 10f);

    [SerializeField, Min(2)]
    int pointsToWin = 3;

    [SerializeField, Min(1f)]
    float newGameDelay = 3f;

    [SerializeField]
    TextMeshProUGUI countdownText;

    float countdownUntilNewGame;

    void Awake() => countdownUntilNewGame = newGameDelay;

    void StartNewGame()
    {
        ball.StartNewGame();
        southPaddle.StartNewGame();
        northPaddle.StartNewGame();
    }

    void Update()
    {
        southPaddle.Move(ball.Position.x, arenaExtents.x);
        northPaddle.Move(ball.Position.x, arenaExtents.x);
        
        if (countdownUntilNewGame <= 0f)
        {
            UpdateGame();
        }
        else
        {
            UpdateCountdown();
        }
    }

    void UpdateGame()
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
        float yExtents = arenaExtents.y - ball.Extents;
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
        
        livelyCamera.PushXZ(ball.Velocity);
        ball.BounceY(boundary);
        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        else
        {
            livelyCamera.JostleY();
            if (attacker.ScorePoint(pointsToWin))
            {
                EndGame();
            }
        }
    }

    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x - ball.Extents;
        if (x < -xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
        }
        else if (x > xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
        }
    }

    void EndGame()
    {
        countdownUntilNewGame = newGameDelay;
        countdownText.SetText("GAME OVER");
        countdownText.gameObject.SetActive(true);
        ball.EndGame();
    }
}
