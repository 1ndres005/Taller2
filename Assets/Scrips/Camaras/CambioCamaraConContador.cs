using UnityEngine;
using Unity.Cinemachine;

public class CambioCamaraConContador : MonoBehaviour
{
    [Header("Referencias de cámaras")]
    public CinemachineCamera camaraA;
    public CinemachineCamera camaraB;

    [Header("Referencia al ItemPickup (contador)")]
    public ItemPickup itemPickup; // arrastra el objeto con tu script ItemPickup aquí

    private bool enCamaraA = true;
    private bool jugadorEnRango = false;

    private void Start()
    {
        // Cámara inicial
        camaraA.Priority = 10;
        camaraB.Priority = 0;
    }

    private void Update()
    {
        // Solo funciona si el jugador está dentro del trigger y presiona E
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            // Si tiene al menos 1 objeto, cambia de cámara
            if (itemPickup != null && itemPickup.pickupCount > 0)
            {
                CambiarCamara();
            }
            else
            {
                Debug.Log("No tienes objetos, no puedes cambiar de cámara.");
            }
        }
    }

    private void CambiarCamara()
    {
        if (enCamaraA)
        {
            camaraA.Priority = 0;
            camaraB.Priority = 10;
        }
        else
        {
            camaraA.Priority = 10;
            camaraB.Priority = 0;
        }

        enCamaraA = !enCamaraA;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log("Presiona E para cambiar cámara (si tienes objetos).");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }
}
