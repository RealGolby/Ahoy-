using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerShootingManager shootingManager;

    Vector3 tmpPos;

    public float TargetZoom = 4;

    Camera camera;

    Vector3 mousePosition;

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
        mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (!shootingManager.Zooming)
        {
            tmpPos = Vector3.Lerp(transform.position, player.transform.position, .125f);
            transform.position = new Vector3(tmpPos.x, tmpPos.y, -10);
        }
        else
        {
            tmpPos = Vector3.Lerp(transform.position, player.transform.position, .125f);

            transform.position = new Vector3(tmpPos.x + mousePosition.x * .5f, tmpPos.y + mousePosition.y * .5f, -10);
        }
    }
}
