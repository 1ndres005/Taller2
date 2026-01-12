using UnityEngine;

public class ArrowBounceSpin3D : MonoBehaviour
{
    [Header("Spin")]
    public float spinSpeed = 120f; // grados por segundo

    [Header("Bounce")]
    public float bounceHeight = 0.25f; // cuánto sube/baja
    public float bounceSpeed = 2.5f;   // qué tan rápido rebota

    [Header("Optional: tilt")]
    public float tiltAngle = 0f; // ej: 10 para inclinarla un poquito

    Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        // 1) Girar sobre el eje Y (dar vuelta)
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

        // 2) Rebotar arriba/abajo (seno)
        float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startLocalPos + new Vector3(0f, yOffset, 0f);

        // 3) Inclinación opcional (queda más "señalando")
        if (tiltAngle != 0f)
        {
            Vector3 e = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(tiltAngle, e.y, 0f);
        }
    }
}
