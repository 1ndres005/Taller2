using UnityEngine;

public class CajonInteractivo : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cajonTransform;   // El cajón que se mueve
    public GameObject uiInteract;      // UI de "Pulsa E"

    [Header("Movimiento del cajón")]
    public Vector3 posicionAbierta = new Vector3(0.3f, 0f, 0f);
    public float velocidad = 3f;

    private Vector3 posicionInicial;
    private bool abierto = false;
    private bool jugadorCerca = false;

    void Start()
    {
        if (cajonTransform == null)
            cajonTransform = transform;

        // Guardar la posición local cuando empieza (posición cerrada)
        posicionInicial = cajonTransform.localPosition;

        // Ocultar UI al inicio
        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        // Interactuar con E si el jugador está cerca
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
            abierto = !abierto;

        // Mover el cajón suave
        if (abierto)
        {
            cajonTransform.localPosition = Vector3.Lerp(
                cajonTransform.localPosition,
                posicionAbierta,
                Time.deltaTime * velocidad
            );
        }
        else
        {
            cajonTransform.localPosition = Vector3.Lerp(
                cajonTransform.localPosition,
                posicionInicial,
                Time.deltaTime * velocidad
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (uiInteract != null)
                uiInteract.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (uiInteract != null)
                uiInteract.SetActive(false);
        }
    }
}
