using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI; // Necesario para usar Slider

public class CambioCamaraSimple : MonoBehaviour
{
    [Header("Referencias de cámaras")]
    public CinemachineCamera camaraA;
    public CinemachineCamera camaraB;

    [Header("Control con Slider")]
    public Slider sliderCamara; // Asigna el Slider desde el Inspector

    private bool yaCambio = false; // Para que solo cambie una vez

    void Start()
    {
        // Escucha el cambio del Slider
        sliderCamara.onValueChanged.AddListener(VerificarCambio);

        // Empezar con la cámara A
        camaraA.Priority = 10;
        camaraB.Priority = 0;
    }

    void VerificarCambio(float valor)
    {
        // Si llega al máximo (100%) y aún no cambió
        if (valor >= sliderCamara.maxValue && !yaCambio)
        {
            camaraA.Priority = 0;
            camaraB.Priority = 10;
            yaCambio = true; // Marca que ya cambió
        }
    }
}
