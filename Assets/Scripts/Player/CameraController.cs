using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 tmpPos;
    void FixedUpdate()
    {
        tmpPos = Vector3.Lerp(transform.position, player.transform.position, .125f);
        transform.position = new Vector3(tmpPos.x,tmpPos.y, -10);
    }
}
