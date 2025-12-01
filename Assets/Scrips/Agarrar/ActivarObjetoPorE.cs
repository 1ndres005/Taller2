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

    void Start()
    {
        // Por si el objeto inicia activo, lo apagamos
        if (objetoMostrar != null)
            objetoMostrar.SetActive(false);
    }

    void Update()
    {
        // --- Activar con E ---
        if (Input.GetKeyDown(tecla))
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
}
