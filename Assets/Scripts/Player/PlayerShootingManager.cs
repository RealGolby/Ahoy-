using UnityEngine;

public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] CameraController cameraController;

    [SerializeField] float Zoom;
    public bool Zooming;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cameraController.TargetZoom = Zoom;
            Zooming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            cameraController.TargetZoom = 5;
            Zooming = false;
        }
    }
}
