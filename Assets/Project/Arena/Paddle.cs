using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Paddle : MonoBehaviour
{
    static readonly int timeOfLastHitId = Shader.PropertyToID("_TimeOfLastHit"),
        faceColorId = Shader.PropertyToID("_FaceColor"),
        emissionColorId = Shader.PropertyToID("_EmissionColor");

    Material goalMaterial, paddleMaterial;

    [SerializeField, Min(0f)]
    float
         minExtents = 4f,
         maxExtents = 4f,
         speed = 10f,
         maxTargetingBias = 0.75f;

    [SerializeField]
    bool isAI;

    [SerializeField]
    MeshRenderer goalRenderer;
    [SerializeField]
    ParticleSystem goalBreakFX;

    [SerializeField]
    ParticleSystem[] souls;

    [SerializeField, ColorUsage(true, true)]
    Color goalColor = Color.white;

    float extents, targetingBias;
    private bool goLeft = false, goRight = false;

    private InputActionsBase baseActions;

    private Material goalBreakFXMat;
    private int initialSouls;
    private float currentZPos;

    [SerializeField]
    int breakParticleEmittionRate = 10;
   
    public int SoulCount { get; private set; }
    public bool IsAI { get => isAI; private set {}}

    public void Setup(float zPos, string name, int initialSoulCount)
    {
        baseActions = new InputActionsBase();

        baseActions.Player.MoveRight.started += ctx => { goRight = true; };
        baseActions.Player.MoveRight.canceled += ctx => { goRight = false; };
        baseActions.Player.MoveLeft.started += ctx => { goLeft = true; };
        baseActions.Player.MoveLeft.canceled += ctx => { goLeft = false; };

        baseActions.Enable();

        goalMaterial = goalRenderer.material;
        goalMaterial.SetColor(emissionColorId, goalColor);
        paddleMaterial = GetComponent<MeshRenderer>().material;

        initialSouls = initialSoulCount;
        SetSouls(initialSouls);

        var renderer = goalBreakFX.GetComponent<ParticleSystemRenderer>();
        goalBreakFXMat = renderer.material;
        goalBreakFXMat.SetColor(emissionColorId, goalColor);

        SetPosition(zPos);
        SetExtents(maxExtents);
    }

    private void SetPosition(float zPos)
    {
        currentZPos = zPos;
        
        transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    currentZPos);
    }

    public void UpdateSoulsFX()
    {
        for (int i = 0; i < souls.Length; i++)
        {
            if (i < SoulCount)
            {
                souls[i].Play();
            }
            else
            {
                souls[i].Stop();
            }
        }
    }

    public void StartNewGame()
    {
           SetSouls(initialSouls);
           ChangeTargetingBias();
           UpdateSoulsFX();
           transform.position = new Vector3(
           0,
           transform.position.y,
           currentZPos);
    }

    void SetExtents(float newExtents)
    {
        extents = newExtents;
        Vector3 scale = transform.localScale;
        scale.x = 2f * newExtents;
        transform.localScale = scale;
    }

    void ChangeTargetingBias() =>
        targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);

    public void TakeDamage()
    {
        goalMaterial.SetFloat(timeOfLastHitId, Time.time);
        SetSouls(SoulCount - 1);
        goalBreakFX.Emit(breakParticleEmittionRate);
        goalBreakFXMat.SetFloat(timeOfLastHitId, Time.time);
        UpdateSoulsFX();
    }

    public bool IsDead()
    {
        return SoulCount <= 0;
    }

    public void Movement(float target, float arenaExtents)
    {
        Vector3 position = transform.localPosition;
        position.x = isAI ? AdjustByAI(position.x, target) : AdjustByPlayer(position.x);
        float limit = arenaExtents - extents;
        position.x = Mathf.Clamp(position.x, -limit, limit);
        transform.localPosition = position;
    }
    float AdjustByAI(float x, float target)
    {
        target += targetingBias * extents;
        if (x < target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);
    }
    float AdjustByPlayer(float x)
    {
        if (goRight && !goLeft)
        {
            return x + speed * Time.deltaTime;
        }
        else if (goLeft && !goRight)
        {
            return x - speed * Time.deltaTime;
        }
        return x;
    }

    public bool HitBallCheck(float ballX, float ballExtents, out float hitFactor)
    {
        hitFactor =
            (ballX - transform.localPosition.x) /
            (extents + ballExtents);
        bool success = -1f <= hitFactor && hitFactor <= 1f;
        if (success)
        {
            paddleMaterial.SetFloat(timeOfLastHitId, Time.time);
        }
        return success;
    }

    void SetSouls(int newSoulCount)
    {
        SoulCount = newSoulCount;

        if (newSoulCount <= 0)return;

        float t = 1f / newSoulCount;
        SetExtents(Mathf.Lerp(maxExtents, minExtents, t));
    }
}
