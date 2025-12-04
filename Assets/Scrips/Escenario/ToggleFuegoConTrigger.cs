using UnityEngine;

public class ToggleFuegoConTrigger : MonoBehaviour
{
    [Header("Arrastra aquí el GameObject del Particle System")]
    public GameObject fuego;

    private bool jugadorDentro = false;
    private bool fuegoEncendido = false;

    void Start()
    {
        if (fuego != null)
            fuego.SetActive(false); // apagado al inicio
    }

    void Update()
    {
        // Solo permite usar Q si el jugador está dentro del trigger
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Q))
        {
            fuegoEncendido = !fuegoEncendido;
            fuego.SetActive(fuegoEncendido);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            Debug.Log("Jugador dentro del trigger, puede encender Q");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            Debug.Log("Jugador fuera del trigger, no puede usar Q");

            // Opcional: si el jugador sale del área, apagar el fuego
            // fuegoEncendido = false;
            // fuego.SetActive(false);
        }
    }
}
