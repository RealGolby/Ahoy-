using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerShootingManager shootingManager;

    Vector3 tmpPos;

    public float TargetZoom = 5;

    Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
        shootingManager = player.GetComponent<PlayerShootingManager>();
    }

    private void Update()
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, TargetZoom, 5 * Time.deltaTime);
    }
    
    void FixedUpdate()
    {
        if (!shootingManager.Zooming)
        {
            tmpPos = Vector3.Lerp(transform.position, player.transform.position, .125f);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, -10);
        }
        else
        {
            tmpPos = Vector3.Lerp(transform.position, player.transform.position, .125f);
            transform.position = new Vector3(tmpPos.x + Camera.main.ScreenToViewportPoint(Input.mousePosition).x / 2, tmpPos.y + Camera.main.ScreenToViewportPoint(Input.mousePosition).y / 2, -10);
        }
    }
}
