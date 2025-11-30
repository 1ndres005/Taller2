using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;        // Velocidad al caminar
    public float runSpeed = 10f;        // Velocidad al correr
    public float rotationSpeed = 15f;   // Qu� tan r�pido gira el personaje

    Rigidbody rb;
    Transform cam;

    Vector2 input;
    bool isRunning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        // Evitar que el personaje se vuelque
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Input WSAD
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Shift para correr
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 moveDirection = cam.forward * input.y + cam.right * input.x;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 targetVelocity = moveDirection * currentSpeed;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );
    }

    void HandleRotation()
    {
        if (input.sqrMagnitude == 0f)
            return;

        Vector3 lookDirection = cam.forward * input.y + cam.right * input.x;
        lookDirection.y = 0f;
        lookDirection.Normalize();

        if (lookDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
