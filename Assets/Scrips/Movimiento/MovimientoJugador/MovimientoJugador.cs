using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Suavidad")]
    public float acceleration = 12f;
    public float rotationSpeed = 15f;

    [Header("Salto")]
    public float jumpForce = 6f;
    public float groundCheckDistance = 0.25f;
    public LayerMask groundLayers;
    public float coyoteTime = 0.10f;

    [Header("Control")]
    public bool SePuedeMover = true;

    Rigidbody rb;
    Transform cam;
    Animator anim;

    Vector2 input;
    bool isRunning;
    bool jumpPressed;

    bool isGrounded;
    float lastGroundedTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (SePuedeMover)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }
        else
        {
            input = Vector2.zero;
        }

        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        if (Input.GetKeyDown(KeyCode.E) && SePuedeMover)
            EjecutarAccion();

        isGrounded = CheckGrounded();

        if (isGrounded)
            lastGroundedTime = Time.time;

        float targetSpeed = input.magnitude * (isRunning ? 1f : 0.5f);
        anim.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);
        anim.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        if (!SePuedeMover) return;

        HandleMovement();
        HandleRotation();
        HandleJump();
    }

    void EjecutarAccion()
    {
        SePuedeMover = false;

        // 🧹 Elimina completamente el impulso residual
        Vector3 stop = rb.linearVelocity;
        stop.x = 0;
        stop.z = 0;
        rb.linearVelocity = stop;

        anim.SetTrigger("Action");
        Invoke(nameof(FinAccion), 0.8f);
    }

    void FinAccion()
    {
        SePuedeMover = true;
    }

    void HandleMovement()
    {
        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();

        Vector3 moveDir = forward * input.y + right * input.x;
        if (moveDir.magnitude > 1f) moveDir.Normalize();

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 targetVelocity = moveDir * speed;

        Vector3 current = rb.linearVelocity;
        Vector3 desired = new Vector3(targetVelocity.x, current.y, targetVelocity.z);

        rb.linearVelocity = Vector3.MoveTowards(current, desired, acceleration * Time.fixedDeltaTime);
    }

    void HandleRotation()
    {
        Vector3 planarVelocity = rb.linearVelocity;
        planarVelocity.y = 0f;

        if (planarVelocity.magnitude < 0.2f) return;

        Quaternion targetRot = Quaternion.LookRotation(planarVelocity.normalized);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
    }

    void HandleJump()
    {
        if (!jumpPressed || !SePuedeMover) return;

        bool canJump = isGrounded || (Time.time - lastGroundedTime) <= coyoteTime;

        if (canJump)
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0;
            rb.linearVelocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpPressed = false;
    }

    bool CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayers);
    }
}
