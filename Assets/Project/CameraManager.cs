using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const float CAMERA_ANGLE = 45f;

    public Camera MainCamera { get; private set; }

    public void Setup(Extents arenaExtents)
    {
        MainCamera = GetComponent<Camera>();
        AdjustCamera(MainCamera, arenaExtents);
    }

    private void AdjustCamera(Camera mainCamera, Extents arenaExtents)
    {
        MainCamera.transform.position = new Vector3(0, arenaExtents._lenght, -arenaExtents._lenght);
    }

}
