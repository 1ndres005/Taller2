using UnityEngine;
using UnityEngine.UI;

public class EspiralControladoConLimite : MonoBehaviour
{
    [Header("Movimiento Circular")]
    public float radio = 2f;                // Distancia fija del centro
    public float suavizado = 8f;            // Suavizado del movimiento
    public float sensibilidadGiro = 1.2f;   // Sensibilidad al movimiento del mouse

    [Header("Carga del Slider")]
    public Slider sliderProgreso;
    public float velocidadCarga = 0.4f;
    public float velocidadDescarga = 0.2f;

    private float anguloActual = 0f;
    private Vector3 posicionInicial;
    private Vector3 centroMouse;
    private Vector3 ultimaPosMouse;
    private bool girando = false;
    private bool iniciado = false;

    void Start()
    {
        posicionInicial = transform.position;

        if (sliderProgreso != null)
        {
            sliderProgreso.value = 0f;
            sliderProgreso.interactable = false;
        }
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // 🔹 Condición: solo si se presionan AMBOS clics
        bool ambosClicsPresionados = Input.GetMouseButton(0) && Input.GetMouseButton(1);

        // 🔸 Inicio: cuando se presionan ambos clics al mismo tiempo
        if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1) ||
            Input.GetMouseButtonDown(1) && Input.GetMouseButton(0))
        {
            centroMouse = mousePos;
            ultimaPosMouse = mousePos;
            iniciado = true;
            girando = false;
        }

        // 🔸 Mantener presionados ambos clics
        if (iniciado && ambosClicsPresionados)
        {
            Vector3 desdeCentroAntes = ultimaPosMouse - centroMouse;
            Vector3 desdeCentroAhora = mousePos - centroMouse;

            float anguloDelta = Vector3.SignedAngle(desdeCentroAntes, desdeCentroAhora, Vector3.forward);
            bool moviendo = desdeCentroAhora.magnitude > 1f && Mathf.Abs(anguloDelta) > 0.1f;

            if (moviendo)
            {
                girando = true;
                anguloActual += anguloDelta * sensibilidadGiro;

                // Cargar el slider mientras gira
                if (sliderProgreso != null)
                    sliderProgreso.value += velocidadCarga * Time.deltaTime;
            }
            else
            {
                girando = false;
                // Descargar si no se mueve el mouse
                if (sliderProgreso != null)
                    sliderProgreso.value -= velocidadDescarga * Time.deltaTime;
            }

            // Movimiento circular alrededor del punto inicial
            float x = Mathf.Cos(anguloActual * Mathf.Deg2Rad) * radio;
            float z = Mathf.Sin(anguloActual * Mathf.Deg2Rad) * radio;

            Vector3 objetivo = posicionInicial + new Vector3(x, 0, z);
            transform.position = Vector3.Lerp(transform.position, objetivo, Time.deltaTime * suavizado);

            ultimaPosMouse = mousePos;
        }
        else
        {
            // 🔸 Si se suelta uno o ambos clics, vuelve al centro
            transform.position = Vector3.Lerp(transform.position, posicionInicial, Time.deltaTime * 2f);

            // Descargar el slider gradualmente
            if (sliderProgreso != null)
                sliderProgreso.value -= velocidadDescarga * Time.deltaTime;
        }

        // 🔸 Limitar el slider entre 0 y 1
        if (sliderProgreso != null)
            sliderProgreso.value = Mathf.Clamp01(sliderProgreso.value);
    }
}
