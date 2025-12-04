using UnityEngine;

public class CameraFollowPlayerXZ : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform jugador;

    [Header("Seguimiento")]
    public float suavizado = 3f;
    public bool usarOffsetInicial = true;
    public Vector3 offsetManual;

    [Header("Control")]
    public bool seguirHabilitado = true;   // <- SimpleCameraDolly lo pondrá en false/true

    Vector3 offset;

    void Start()
    {
        if (jugador == null)
        {
            Debug.LogWarning("CameraFollowPlayerXZ: no hay jugador asignado.");
            return;
        }

        if (usarOffsetInicial)
        {
            // Calculamos offset manteniendo la altura actual de la cámara
            Vector3 jugadorXZ = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
            offset = transform.position - jugadorXZ;
        }
        else
        {
            offset = offsetManual;
        }
    }

    void LateUpdate()
    {
        if (!seguirHabilitado || jugador == null)
            return;

        // Seguir solo en XZ, manteniendo la Y de la cámara
        Vector3 jugadorXZ = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
        Vector3 destino = jugadorXZ + offset;

        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }
}
