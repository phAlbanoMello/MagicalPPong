using UnityEngine;

public class CameraMotionEffects : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float
        springStrength = 100f,
        dampingStrength = 10f,
        jostleStrength = 40f,
        pushStrength = 1f,
        maxDeltaTime = 1f / 60f;
    
    Vector3 anchorPosition, velocity;

    void Awake() => anchorPosition = transform.localPosition;

    public void JostleY() => velocity.y += jostleStrength;

    public void PushXZ(Vector2 impulse)
    {
        velocity.x += pushStrength * impulse.x;
        velocity.z += pushStrength * impulse.y;
    }

    void LateUpdate()
    {
        float deltaTime = Time.deltaTime;
        while (deltaTime > maxDeltaTime)
        {
            TimeStep(maxDeltaTime);
            deltaTime -= maxDeltaTime;
        }
        TimeStep(deltaTime); //Adjusts spring motion velocity proportional
                             //to the time spent between frames.
    }

    void TimeStep(float deltaTime)
    {
        Vector3 displacement = anchorPosition - transform.localPosition;
        Vector3 acceleration = springStrength * displacement - dampingStrength * velocity;
        velocity += acceleration * deltaTime;
        transform.localPosition += velocity * deltaTime;
    }
}
