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
    public bool activarPorSlider = false;
    public Slider sliderControl;
    public float valorSliderObjetivo = 1f;

    [Header("Bloquear movimiento de cámara durante el dolly")]
    public CameraFollowPlayerXZ cameraFollow;

    [Header("Uso único")]
    public bool usarSoloUnaVez = false;

    [Header("Objetos a desaparecer (uso único)")]
    public GameObject[] objetosADesaparecer;

    bool jugadorCerca = false;
    bool enCinematica = false;
    bool camaraArriba = false;

    // para que el slider solo dispare una vez
    bool sliderDisparado = false;

    Vector3 posicionInicial;
    Quaternion rotacionInicial;

    // ✅ NUEVO: la UI se bloquea SOLO después de interactuar (cuando usarSoloUnaVez + activarPorSlider)
    bool interaccionHecha = false;

    // ✅ NUEVO: si activarPorSlider está activo, E solo una vez
    bool eYaUsadaConSlider = false;

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
            if (!sliderDisparado && sliderControl.value >= valorSliderObjetivo)
            {
                sliderDisparado = true;

                // ✅ si está en modo "una vez" + slider, al interactuar bloqueamos la UI para siempre
                if (usarSoloUnaVez)
                {
                    interaccionHecha = true;
                    DesaparecerObjetosUnaVez();
                }

                if (uiInteract != null)
                    uiInteract.SetActive(false);

                if (!camaraArriba)
                    StartCoroutine(MoverCamaraIda());
                else
                    StartCoroutine(MoverCamaraVuelta());

                return;
            }

            if (sliderDisparado && sliderControl.value < valorSliderObjetivo * 0.1f)
            {
                sliderDisparado = false;
            }
        }

        // 🔥 MODO MANUAL (tecla E, solo si NO está en automático de trigger)
        if (!cambioAutomatico && jugadorCerca && !enCinematica && Input.GetKeyDown(KeyCode.E))
        {
            // ✅ Si activarPorSlider está activo, E solo se puede usar una vez
            if (activarPorSlider && eYaUsadaConSlider)
                return;

            if (activarPorSlider)
                eYaUsadaConSlider = true;

            // ✅ si está en modo "una vez" + slider, al interactuar bloqueamos la UI para siempre
            if (activarPorSlider && usarSoloUnaVez)
            {
                interaccionHecha = true;
                DesaparecerObjetosUnaVez();
            }

            // ✅ ocultar UI al interactuar
            if (uiInteract != null)
                uiInteract.SetActive(false);

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

        if (cameraFollow != null)
            cameraFollow.seguirHabilitado = true;

        // ✅ Solo mostrar UI si NO estamos bloqueados por "una vez + slider"
        if (!cambioAutomatico && jugadorCerca && uiInteract != null)
        {
            if (!(activarPorSlider && usarSoloUnaVez && interaccionHecha))
                uiInteract.SetActive(true);
        }
    }

    // ==========================
    //      TRIGGERS
    // ==========================
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        jugadorCerca = true;

        if (cambioAutomatico && !enCinematica)
        {
            if (!camaraArriba)
                StartCoroutine(MoverCamaraIda());
            else
                StartCoroutine(MoverCamaraVuelta());

            return;
        }

        // ✅ Si "una vez + slider" y ya interactuó, NO mostrar nunca más
        if (activarPorSlider && usarSoloUnaVez && interaccionHecha)
            return;

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

    // ==========================
    //  OBJETOS A DESAPARECER
    // ==========================
    void DesaparecerObjetosUnaVez()
    {
        if (objetosADesaparecer == null) return;

        foreach (GameObject obj in objetosADesaparecer)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
