using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;

    public int JumpsRemaining;

    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] float jumpCooldown;

    [SerializeField] int dashStaminaCost;
    [SerializeField] int doubleJumpStaminaCost;

    [SerializeField] float groundDrag;

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
    Vector2 mouseOnScreen;

    [SerializeField] GameObject PlayerHead;
    Player player;

    [SerializeField] float staminaRecoveryTimer;
    [SerializeField] float time;
    [SerializeField] bool canRecoverStamina;
    [SerializeField] bool recoveringStamina;

    IEnumerator recoverStamianCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        player = GetComponent<Player>();

        recoverStamianCoroutine = regenerateStamina();
    }
    private void Update()
    {
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRotation, 8/*.075f*/ * Time.deltaTime);

        getInput();
        speedControl();
        rotateHead();
        rotateBody();
        recoverStamina();

        if (isGrounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 1.5f), .05f, groundLayer);
        move();
    }

    void recoverStamina()
    {
        if (time > 0)
        {
            Debug.Log("Delaying stamina recovery");
            time -= Time.deltaTime;
            if (time <= 0 && !recoveringStamina) canRecoverStamina = true;
        }

        if (canRecoverStamina && !recoveringStamina && player.Stamina < player.MaxStamina) StartCoroutine(regenerateStamina());
    }

    IEnumerator regenerateStamina()
    {
        Debug.Log("Coroutine regeneretate stmaina");
        recoveringStamina = true;
        if (player.Stamina < player.MaxStamina)
        {
            yield return new WaitForSeconds(.5f);
            player.ChangeStamina(5);
        }
        recoveringStamina = false;
    }

    void getInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if (isGrounded) JumpsRemaining = 1;

        if (Input.GetKeyDown(KeyCode.Space)) jump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && player.Stamina >= dashStaminaCost) StartCoroutine(dash()); ;
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
        else if (!isGrounded && canJump && JumpsRemaining > 0 && player.Stamina >= 5)
        {
            time = staminaRecoveryTimer;
            if (recoverStamianCoroutine != null) StopCoroutine(recoverStamianCoroutine);
            recoveringStamina = false;
            canRecoverStamina = false;

            player.ChangeStamina(-doubleJumpStaminaCost);
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
    }

    void rotateHead()
    {

        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        if (angle > 90) angle = 180 - angle;
        else if (angle < -90) angle = 180 - angle;
        //Debug.Log(angle);

        PlayerHead.transform.localRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, angle/* Mathf.Clamp(angle,-90,90)*/));
    }

    void rotateBody()
    {
        if (mouseOnScreen.x > .5f)
        {
            targetRotation = new Vector3(0, 180, 0);
        }
        else
        {
            targetRotation = new Vector3(0, 0, 0);
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
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
        time = staminaRecoveryTimer;
        if (recoverStamianCoroutine != null) StopCoroutine(recoverStamianCoroutine);
        recoveringStamina = false;
        canRecoverStamina = false;

        player.ChangeStamina(-dashStaminaCost);
        canDash = false;
        dashing = true;
        rb.AddForce(new Vector2(dashForce, 0), ForceMode2D.Force);
        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        dashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
    }
}
