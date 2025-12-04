using UnityEngine;
using UnityEngine.UI;

public class SimpleCameraDollySlider : MonoBehaviour
{
    [Header("Cámara")]
    public Transform cameraTransform;

    [Header("Destino del dolly")]
    public Transform puntoDestino;
    public float duracionIda = 2f;
    public float duracionVuelta = 2f;

    [Header("Interacción")]
    public GameObject uiInteract;

    [Header("Activación automática")]
    public bool cambioAutomatico = false;

    [Header("Activación por Slider")]
    public bool activarPorSlider = false;     // si true, el slider puede activar el movimiento
    public Slider sliderControl;
    public float valorSliderObjetivo = 1f;    // cuando llega a este valor, se dispara

    [Header("Bloquear movimiento de cámara durante el dolly")]
    public CameraFollowPlayerXZ cameraFollow; // <- script que mueve la cámara siguiendo al jugador

    bool jugadorCerca = false;
    bool enCinematica = false;

    bool camaraArriba = false;

    // para que el slider solo dispare una vez
    bool sliderDisparado = false;

    Vector3 posicionInicial;
    Quaternion rotacionInicial;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        // 🔥 ACTIVACIÓN POR SLIDER (si está activada)
        if (activarPorSlider && sliderControl != null && !enCinematica)
        {
            // Si el slider llegó al objetivo y aún no ha disparado
            if (!sliderDisparado && sliderControl.value >= valorSliderObjetivo)
            {
                sliderDisparado = true; // marcamos que ya lo usamos

                if (!camaraArriba)
                    StartCoroutine(MoverCamaraIda());
                else
                    StartCoroutine(MoverCamaraVuelta());

                // salimos para no mezclar con lo demás este frame
                return;
            }

            // Si el slider fue reiniciado (por ejemplo lo pones en 0 desde otro script),
            // lo rearmamos para que pueda volver a disparar.
            if (sliderDisparado && sliderControl.value < valorSliderObjetivo * 0.1f)
            {
                sliderDisparado = false;
            }
        }

        // 🔥 MODO MANUAL (tecla E, solo si NO está en automático de trigger)
        if (!cambioAutomatico && jugadorCerca && !enCinematica && Input.GetKeyDown(KeyCode.E))
        {
            if (!camaraArriba)
                StartCoroutine(MoverCamaraIda());
            else
                StartCoroutine(MoverCamaraVuelta());
        }
    }

    // ==========================
    //        IDA
    // ==========================
    System.Collections.IEnumerator MoverCamaraIda()
    {
        enCinematica = true;

        // 🔒 Bloquear follow de cámara mientras entra al punto/dolly
        if (cameraFollow != null)
            cameraFollow.seguirHabilitado = false;

        posicionInicial = cameraTransform.position;
        rotacionInicial = cameraTransform.rotation;

        Vector3 inicioPos = cameraTransform.position;
        Quaternion inicioRot = cameraTransform.rotation;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / Mathf.Max(duracionIda, 0.0001f);

            cameraTransform.position = Vector3.Lerp(inicioPos, puntoDestino.position, t);
            cameraTransform.rotation = Quaternion.Slerp(inicioRot, puntoDestino.rotation, t);

            yield return null;
        }

        cameraTransform.position = puntoDestino.position;
        cameraTransform.rotation = puntoDestino.rotation;

        camaraArriba = true;
        enCinematica = false;
    }

    // ==========================
    //        VUELTA
    // ==========================
    System.Collections.IEnumerator MoverCamaraVuelta()
    {
        enCinematica = true;

        Vector3 inicioPos = cameraTransform.position;
        Quaternion inicioRot = cameraTransform.rotation;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / Mathf.Max(duracionVuelta, 0.0001f);

            cameraTransform.position = Vector3.Lerp(inicioPos, posicionInicial, t);
            cameraTransform.rotation = Quaternion.Slerp(inicioRot, rotacionInicial, t);

            yield return null;
        }

        cameraTransform.position = posicionInicial;
        cameraTransform.rotation = rotacionInicial;

        camaraArriba = false;
        enCinematica = false;

        // 🔓 Volver a permitir que la cámara siga al jugador
        if (cameraFollow != null)
            cameraFollow.seguirHabilitado = true;

        if (!cambioAutomatico && jugadorCerca && uiInteract != null)
            uiInteract.SetActive(true);
    }

    // ==========================
    //      TRIGGERS
    // ==========================
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = true;

        // 🔥 MODO AUTOMÁTICO por trigger
        if (cambioAutomatico && !enCinematica)
        {
            if (!camaraArriba)
                StartCoroutine(MoverCamaraIda());
            else
                StartCoroutine(MoverCamaraVuelta());

            return;
        }

        // 🔥 MODO MANUAL (solo E)
        if (!enCinematica && uiInteract != null)
            uiInteract.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = false;

        if (!cambioAutomatico && uiInteract != null)
            uiInteract.SetActive(false);
    }
}
