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

    bool effectsEnabled = true;

    public void Setup() {
        anchorPosition = transform.localPosition;
    }

    public void JostleY() => velocity.y += jostleStrength;

    public void PushXZ(Vector2 impulse)
    {
        velocity.x += pushStrength * impulse.x;
        velocity.z += pushStrength * impulse.y;
    }
    public void ToggleEnabled(bool value)
    {
        effectsEnabled = value;
    }
    void LateUpdate()
    {
        if (effectsEnabled)
        {
            float deltaTime = Time.deltaTime;
            while (deltaTime > maxDeltaTime)
            {
                TimeStep(maxDeltaTime);
                deltaTime -= maxDeltaTime;
            }
            TimeStep(deltaTime);
        }
    }

    void TimeStep(float deltaTime)
    {
        Vector3 displacement = anchorPosition - transform.localPosition;
        Vector3 acceleration = springStrength * displacement - dampingStrength * velocity;
        velocity += acceleration * deltaTime;
        transform.localPosition += velocity * deltaTime;
    }
}
