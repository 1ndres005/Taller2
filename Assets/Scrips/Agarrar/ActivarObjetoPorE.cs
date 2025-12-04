using UnityEngine;
using UnityEngine.UI;

public class ActivarObjetoPorE : MonoBehaviour
{
    [Header("Objeto que aparecerá al presionar E")]
    public GameObject objetoMostrar;

    [Header("Slider que controla la desaparición")]
    public Slider sliderControl;

    [Header("Tecla de activación")]
    public KeyCode tecla = KeyCode.E;

    bool jugadorDentro = false;

    void Start()
    {
        if (objetoMostrar != null)
            objetoMostrar.SetActive(false);
    }

    void Update()
    {
        // --- Activar con E SOLO si está dentro del trigger ---
        if (jugadorDentro && Input.GetKeyDown(tecla))
        {
            if (objetoMostrar != null)
                objetoMostrar.SetActive(true);
        }

        // --- Desactivar cuando el slider llegue a 1 ---
        if (objetoMostrar != null && sliderControl != null)
        {
            if (sliderControl.value >= 1f)
            {
                objetoMostrar.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            // Debug.Log("Jugador dentro del trigger.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            // Debug.Log("Jugador fuera del trigger.");
        }
    }
}
