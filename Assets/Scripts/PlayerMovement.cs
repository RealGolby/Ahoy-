using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Idle, Moving, Running, Dashing, Dead
}

public enum PlayerFace
{
    Left, Right, Up, Down
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;

    [SerializeField] float dashCooldown;

    [SerializeField] PlayerState playerState;
    [SerializeField] PlayerFace playerFace;

    Rigidbody2D rb;
    SpriteRenderer sr;

    [SerializeField] bool isGrounded;

    [SerializeField] LayerMask groundLayer;
    float moveX;

    bool canJump;
    bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (playerState == PlayerState.Dead) return;

        jump();
        move();
        dash();
    }

    void jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        if (hit.collider == null) isGrounded = false;
        else
        {
            isGrounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce));
            }
            else if (canJump && !isGrounded)
            {
                canJump = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, jumpForce * 1.5f));
            }
        }


    }
    void move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0)
        {
            playerState = PlayerState.Moving;
            if (isGrounded)
            {
                transform.position += new Vector3(moveX * movementSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position += new Vector3(moveX / 1.5f * movementSpeed * Time.deltaTime, 0, 0);
            }

            if (moveX > 0)
            {
                sr.flipX = false;
                playerFace = PlayerFace.Right;
            }
            else if (moveX < 0)
            {
                sr.flipX = true;
                playerFace = PlayerFace.Left;
            }
        }
        else
        {
            Invoke("setPlayerIdle",.5f);
        }
    }

    void setPlayerIdle()
    {
        playerState = PlayerState.Idle;
    }

    void dash()
    {
        if (playerState != PlayerState.Moving) return;
        if (playerFace == PlayerFace.Right && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dashing");
            playerState = PlayerState.Dashing;
            rb.AddForce(new Vector2(dashForce * 1, 0));
            StartCoroutine("getDashCooldown");
        }
        else if (playerFace == PlayerFace.Left && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dashing");
            playerState = PlayerState.Dashing;
            rb.AddForce(new Vector2(dashForce * -1, 0));
            StartCoroutine("getDashCooldown");
        }
    }

    IEnumerator getDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        playerState = PlayerState.Idle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("dd");
            if(!canJump)
            canJump = true;
        }
    }
}
