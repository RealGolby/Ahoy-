using System.Collections;
using UnityEngine;

public enum PlayerFace
{
    Left, Right, Up, Down
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;

    public int JumpsRemaining;

    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] float jumpCooldown;

    [SerializeField] float groundDrag;

    [SerializeField] PlayerFace playerFace;

    Rigidbody2D rb;
    SpriteRenderer sr;
    TrailRenderer tr;

    [SerializeField] bool isGrounded;

    [SerializeField] LayerMask groundLayer;
    float moveX;

    bool canJump = true;
    bool canDash = true;
    bool dashing = false;

    Vector3 targetRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
    }
    private void Update()
    {
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRotation,.075f);

        getInput();
        speedControl();

        if (isGrounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 1.5f), .2f, groundLayer);
        move();
    }

    void getInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if (isGrounded) JumpsRemaining = 1;

        if (Input.GetKeyDown(KeyCode.Space)) jump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) StartCoroutine(dash()); ;
    }

    void jump()
    {
        if (isGrounded && canJump)
        {
            canJump = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            JumpsRemaining--;
            Invoke("delayJump", jumpCooldown);
        }
        else if (!isGrounded && canJump && JumpsRemaining > 0)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            JumpsRemaining--;
            Invoke("delayJump", jumpCooldown);
        }
    }

    void delayJump()
    {
        canJump = true;
    }

    void move()
    {
        if (moveX == 0) return;

        rb.AddForce(new Vector2(moveX * movementSpeed * 10, 0), ForceMode2D.Force);

        if (moveX > 0)
        {
            targetRotation = new Vector3(0,180,0);
            playerFace = PlayerFace.Right;
        }
        else if (moveX < 0)
        {
            targetRotation = new Vector3(0, 0, 0);
            playerFace = PlayerFace.Left;
        }
    }

    private void speedControl()
    {
        if (dashing) return;
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f);
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y);
        }
    }

    IEnumerator dash()
    {
        canDash = false;
        dashing = true;
        rb.AddForce(new Vector2(dashForce,0),ForceMode2D.Force);
        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        dashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
    }
}