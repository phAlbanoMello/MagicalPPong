using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float
         maxStartXSpeed = 2f,
         maxXSpeed = 20f,
         constantYSpeed = 10f,
         extents = 0.5f;

    Vector2 position, velocity;
    public float Extents => extents;

    public Vector2 Position => position;
    public Vector2 Velocity => velocity;
   
    [Header ("VFX")]
    [SerializeField]
    ParticleSystem bounceParticleSystem, startParticleSystem, trailParticleSystem;

    [SerializeField]
    int bounceParticleEmission = 20,
        startParticleEmission = 100;

    public void UpdateVisualization() => trailParticleSystem.transform.localPosition =
        transform.localPosition = new Vector3(position.x, 0f, position.y);

    public void Move() => position += velocity * Time.deltaTime;

    public void StartNewGame()
    {
        position = Vector2.zero;
        UpdateVisualization();
        velocity.x = Random.Range(-maxStartXSpeed, maxStartXSpeed);
        velocity.y = -constantYSpeed;
        gameObject.SetActive(true);
        startParticleSystem.Emit(startParticleEmission);
        SetTrailEmission(true);
        trailParticleSystem.Play();
    }

    public void BounceX(float boundary)
    {
        position.x = 2f * boundary - position.x;
        velocity.x = -velocity.x;
        EmitBounceParticles(boundary < 0f ? 90f : 270f);
    }

    public void BounceY(float boundary)
    {
        position.y = 2f * boundary - position.y;
        velocity.y = -velocity.y;
        EmitBounceParticles(boundary < 0f ? 0f : 180f);
    }

    public void SetXPositionAndSpeed(float start, float speedFactor, float deltaTime)
    {
        velocity.x = maxXSpeed * speedFactor;
        position.x = start + velocity.x * deltaTime;
    }
    public void EndGame()
    {
        position.x = 0f;
        gameObject.SetActive(false);
        SetTrailEmission(false);
    }

    void EmitBounceParticles(float rotation)
    {
        ParticleSystem.ShapeModule shape = bounceParticleSystem.shape;
        shape.rotation = new Vector3(0f, rotation, 0f);
        bounceParticleSystem.Emit(bounceParticleEmission);
    }
    void SetTrailEmission(bool enabled)
    {
        ParticleSystem.EmissionModule emission = trailParticleSystem.emission;
        emission.enabled = enabled;
    }
}
