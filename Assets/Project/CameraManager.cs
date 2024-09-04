using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera GameCamera;

    public void Setup(Extents arenaExtents)
    {
        AdjustCamera(GameCamera, arenaExtents);
        SetupCameraEffectsAnchor();
    }

    private void AdjustCamera(Camera mainCamera, Extents arenaExtents)
    {
        GameCamera.transform.position = new Vector3(0, arenaExtents._length, -arenaExtents._length);
    }

    private void SetupCameraEffectsAnchor()
    {
        CameraMotionEffects camFx = GameCamera.GetComponent<CameraMotionEffects>();

        if (camFx == null)return;
     
        camFx.Setup();
    }

}
