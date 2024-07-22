using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const float CAMERA_ANGLE = 45f;

    private Camera mainCamera;

    public void Init(Extents arenaExtents)
    {
        mainCamera = GetComponent<Camera>();
        AdjustCamera(mainCamera, arenaExtents);
        //AdjustCameraPosition(Camera.main, arenaExtents);
    }

    public void AdjustCameraPosition(Camera mainCamera, Extents arenaExtents)
    {
        float arenaDiagonal = Mathf.Sqrt(
            Mathf.Pow(arenaExtents._lenght, 2) + Mathf.Pow(arenaExtents._width, 2)
        );

        Vector3 cameraTargetPosition = new Vector3(
            0f,
            arenaDiagonal / Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2f) / 2, 
            -arenaDiagonal / 2
        );

        Quaternion cameraTargetRotation = Quaternion.Euler(CAMERA_ANGLE, 0f, 0f);

        mainCamera.transform.position = cameraTargetPosition;
        mainCamera.transform.rotation = cameraTargetRotation;
    }

    private void AdjustCamera(Camera mainCamera, Extents arenaExtents)
    {
        mainCamera.transform.position = new Vector3(0, arenaExtents._lenght, -arenaExtents._lenght);
    }

}
