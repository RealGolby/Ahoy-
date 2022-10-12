using UnityEngine;

public enum PlayerState
{
    Idle, Moving, Running, Dead
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] PlayerState playerState;

    Rigidbody2D rb;
    [SerializeField] bool isGrounded;

    [SerializeField] LayerMask groundLayer;

    public int JumpsRemaining = 2;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (playerState == PlayerState.Dead) return;

        jump();
        move();
    }

    void jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        if (hit.collider == null) isGrounded = false;
        else {
            isGrounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce));
                JumpsRemaining--;
            }
            else if(JumpsRemaining > 0 && !isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce * 1.5f));
                JumpsRemaining--;
            }
        }
     

    }
    void move()
    {
        float moveX = Input.GetAxis("Horizontal");

        if(moveX > 0)
        {
            playerState = PlayerState.Moving;
            if (isGrounded)
            {
                transform.position += new Vector3(moveX/2 * movementSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position += new Vector3(moveX * movementSpeed * Time.deltaTime, 0, 0);
            }

        }
        else
        {
            playerState = PlayerState.Idle;   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            JumpsRemaining = 2;
            Debug.Log(":)");
        }
    }
}
