using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JugadorFPS : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 9f;
    public float fuerzaSalto = 5f;
    public float gravedad = -9.81f;

    [Header("Mouse Look")]
    public Transform camara;
    public float sensibilidadMouse = 2f;

    [Header("Head Bob")]
    public float amplitudBob = 0.05f;
    public float frecuenciaBob = 6f;

    [Header("Respiración")]
    public float amplitudRespirar = 0.015f; // qué tanto se mueve al respirar
    public float frecuenciaRespirar = 1.2f; // qué tan rápido "respira"

    private CharacterController controller;
    private Vector3 velocidad;
    private float rotacionX = 0f;
    private Vector3 posicionCamaraInicial;
    private float tiempoBob;
    private float tiempoRespirar;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        posicionCamaraInicial = camara.localPosition;
    }

    void Update()
    {
        Mirar();
        Mover();
        MovimientoCamara();
    }

    void Mover()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool corriendo = Input.GetKey(KeyCode.LeftShift);
        float velocidadActual = corriendo ? velocidadCorrer : velocidadCaminar;

        Vector3 mover = transform.right * x + transform.forward * z;
        mover *= velocidadActual;

        // Salto
        if (controller.isGrounded)
        {
            velocidad.y = -2f;
            if (Input.GetButtonDown("Jump"))
                velocidad.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        // Gravedad
        velocidad.y += gravedad * Time.deltaTime;

        // Movimiento total
        controller.Move((mover + new Vector3(0, velocidad.y, 0)) * Time.deltaTime);
    }

    void Mirar()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void MovimientoCamara()
    {
        bool seMueve = controller.isGrounded && new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude > 0.1f;

        if (seMueve)
        {
            // Headbob de caminar/correr
            float frecuencia = (Input.GetKey(KeyCode.LeftShift)) ? frecuenciaBob * 1.4f : frecuenciaBob;
            tiempoBob += Time.deltaTime * frecuencia;

            float desplazamientoY = Mathf.Sin(tiempoBob) * amplitudBob;
            float desplazamientoX = Mathf.Cos(tiempoBob / 2f) * amplitudBob / 2f;

            camara.localPosition = posicionCamaraInicial + new Vector3(desplazamientoX, desplazamientoY, 0f);
        }
        else
        {
            // Respiración cuando está quieto
            tiempoRespirar += Time.deltaTime * frecuenciaRespirar;
            float desplazamientoY = Mathf.Sin(tiempoRespirar) * amplitudRespirar;
            camara.localPosition = Vector3.Lerp(
                camara.localPosition,
                posicionCamaraInicial + new Vector3(0, desplazamientoY, 0),
                Time.deltaTime * 4f
            );

            tiempoBob = 0f;
        }
    }
}
