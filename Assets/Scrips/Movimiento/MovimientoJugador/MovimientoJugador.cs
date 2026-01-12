using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Suavidad")]
    public float acceleration = 20f;
    public float rotationSpeed = 15f;

    [Header("Salto")]
    public float jumpForce = 6f;
    public float groundCheckDistance = 0.25f;
    public LayerMask groundLayers;
    public float coyoteTime = 0.10f;

    Rigidbody rb;
    Transform cam;

    Vector2 input;
    bool isRunning;

    bool jumpPressed;
    float lastGroundedTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // ✅ FIX IMPORTANTE:
        // NO sobrescribe las constraints del Inspector
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Input suave
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Salto con espacio
        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        // Guardar último momento en suelo (coyote time)
        if (IsGrounded())
            lastGroundedTime = Time.time;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
    }

    void HandleMovement()
    {
        Vector3 forward = cam.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDirection = forward * input.y + right * input.x;
        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 desiredVelocity = moveDirection * speed;

        Vector3 currentVel = rb.linearVelocity;
        Vector3 targetVel = new Vector3(desiredVelocity.x, currentVel.y, desiredVelocity.z);

        rb.linearVelocity = Vector3.MoveTowards(
            currentVel,
            targetVel,
            acceleration * Time.fixedDeltaTime
        );
    }

    void HandleRotation()
    {
        if (input.sqrMagnitude < 0.001f) return;

        Vector3 forward = cam.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0f;
        right.Normalize();

        Vector3 lookDir = forward * input.y + right * input.x;
        if (lookDir.sqrMagnitude < 0.001f) return;

        lookDir.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
        Quaternion newRot = Quaternion.Slerp(
            rb.rotation,
            targetRot,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newRot);
    }

    void HandleJump()
    {
        if (!jumpPressed) return;

        bool groundedOrCoyote =
            IsGrounded() || (Time.time - lastGroundedTime) <= coyoteTime;

        if (groundedOrCoyote)
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpPressed = false;
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
    }
#endif
}
