using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Paddle : MonoBehaviour
{
    static readonly int timeOfLastHitId = Shader.PropertyToID("_TimeOfLastHit"), 
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
    TextMeshProUGUI scoreText;

    [SerializeField]
    MeshRenderer goalRenderer;

    [SerializeField, ColorUsage(true, true)]
    Color goalColor = Color.white;

    int score;

    float extents, targetingBias;

    void Awake()
    {
        goalMaterial = goalRenderer.material;
        goalMaterial.SetColor(emissionColorId, goalColor);
        paddleMaterial = GetComponent<MeshRenderer>().material;
        SetScore(0);
    }

    public void StartNewGame()
    {
           SetScore(0);
           ChangeTargetingBias();
    }

    void SetExtents(float newExtents)
    {
        extents = newExtents;
        Vector3 s = transform.localScale;
        s.x = 2f * newExtents;
        transform.localScale = s;
    }

    void ChangeTargetingBias() =>
        targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);

    public bool ScorePoint(int pointsToWin)
    {
        goalMaterial.SetFloat(timeOfLastHitId, Time.time);
        SetScore(score + 1, pointsToWin);
        return score >= pointsToWin;
    }

    public void Move(float target, float arenaExtents)
    {
        Vector3 p = transform.localPosition;
        p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
        float limit = arenaExtents - extents;
        p.x = Mathf.Clamp(p.x, -limit, limit);
        transform.localPosition = p;
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
        bool goRight = Input.GetKey(KeyCode.RightArrow);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow);
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

    public bool HitBall(float ballX, float ballExtents, out float hitFactor)
    {
        hitFactor =
            (ballX - transform.localPosition.x) /
            (extents + ballExtents);
        bool success = -1f <= hitFactor && hitFactor <= 1f;
        if (success)
        {
            Debug.Log("Time : " + Time.time);
            paddleMaterial.SetFloat(timeOfLastHitId, Time.time);
            Debug.Log("Time of Last Hit : " + timeOfLastHitId);
        }
        return success;
    }

    void SetScore(int newScore, float pointsToWin = 1000f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointsToWin - 1f)));
    }

}
