using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    bool cameraEffects;

    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle southPaddle, northPaddle;

    [SerializeField, Min(2)]
    int initialSouls = 5;

    [SerializeField, Min(1f)]
    float newGameDelay = 3f;

    [SerializeField]
    TextMeshProUGUI countdownText;

    private CameraMotionEffects CamMotionFX;
    private SoundManager soundManager;
    private InputActionsBase baseActions;


    private Vector2 arenaExtents;
    private float countdownUntilNewGame;

    private readonly string WIN_MESSAGE = "ENEMY DEFEATED";
    private readonly string LOSE_MESSAGE = "YOU DIED";
    private readonly string AI_NAME = "AI";
    private readonly string PLAYER_NAME = "Player";

    public bool RunUpdate { get; private set; }
    public bool CameraEffects { get => cameraEffects; private set { } }

    public void Init(Extents extents, SoundManager soundManager)
    {
        this.soundManager = soundManager;
        ball.gameObject.SetActive(false);
        CamMotionFX = Camera.main.GetComponent<CameraMotionEffects>();
        UpdateArenaExtents(extents);

        northPaddle.Setup(arenaExtents.y/2, AI_NAME, initialSouls);
        southPaddle.Setup(-arenaExtents.y/2, PLAYER_NAME, initialSouls);

        StartCountdown();   
    }

    public void UpdateArenaExtents(Extents extents)
    {
        arenaExtents = new Vector2(extents._width, extents._length);
        northPaddle.Setup(arenaExtents.y / 2, AI_NAME, initialSouls);
        southPaddle.Setup(-arenaExtents.y / 2, PLAYER_NAME, initialSouls);
    }

    public void UpdateDifficulty()
    {
        northPaddle.UpdateSpeed(GameSettings.Instance.Difficulty);
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

        southPaddle.Movement(ball.Position.x, arenaExtents.x/2);
        northPaddle.Movement(ball.Position.x, arenaExtents.x/2);

        UpdateBall();
    }

    public void ResetGame()
    { 
        StartNewGame();
        ball.gameObject.SetActive(false);
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

        if(cameraEffects)CamMotionFX.PushXZ(ball.Velocity);

        ball.BounceY(boundary);
  
        if (defender.HitBallCheck(bounceX, ball.Extents, out float hitFactor))
        {
            soundManager.PlaySound("Hit");
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        else
        {
            if (cameraEffects) CamMotionFX.JostleY();
            attacker.TakeDamage();
            soundManager.PlaySound("Damage");
            if (attacker.IsDead())
            {
                EndGame(attacker);
            }else if (defender.IsDead())
            {
                EndGame(defender);
            }
        }
    }

    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x/2 - ball.Extents;
        if (x < -xExtents) //Is hitting the left side of the arena
        {
            if (cameraEffects) CamMotionFX.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
            soundManager.PlaySound("Bounce");
        }
        else if (x > xExtents)//Is hitting the right side of the arena
        {
            if (cameraEffects) CamMotionFX.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
            soundManager.PlaySound("Bounce");
        }
    }

    void EndGame(Paddle winner)
    {
        soundManager.PlaySound("GameOver");
        StartCountdown();
        
        string gameOverText = winner.IsAI ? LOSE_MESSAGE : WIN_MESSAGE;
        Color gameOverColor = winner.IsAI ? Color.red : Color.blue;

        countdownText.fontMaterial.SetColor("_EmissionColor", gameOverColor);
        countdownText.SetText(gameOverText);
        countdownText.gameObject.SetActive(true);
        ball.EndGame();
    }

}
