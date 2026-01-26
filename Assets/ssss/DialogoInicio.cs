using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogoInicio : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public Image imagenDialogo;
    private CanvasGroup imagenCanvasGroup;

    [Header("Objetos a desaparecer al terminar el diálogo")]
    public GameObject[] objetosADesaparecer;

    [Header("Contenido del diálogo (editable en Inspector)")]
    [TextArea(3, 6)]
    public string[] lineas;

    [Header("Imágenes para cada línea")]
    public Sprite[] imagenes;

    [Header("Configuración Fade")]
    public float duracionFade = 0.25f;

    [Header("Configuración Texto")]
    public float delayPalabra = 0.25f;
    public float shakeIntensidad = 5f;
    public float shakeDuracion = 0.3f;

    [Header("Dolly (volver cámara al terminar)")]
    public CameraDolly_M_y_VuelveSiObjetoDesaparece cameraDolly;

    private int indiceLinea = 0;
    private bool dialogoActivo = false;
    private bool esperandoInput = false; // ✅ NUEVO
    private Coroutine fadeCoroutine;
    private Coroutine mostrarTextoCoroutine;

    private MovimientoJugador movimientoJugador;
    private Rigidbody rbPlayer;

    void Awake()
    {
        imagenCanvasGroup = imagenDialogo.GetComponent<CanvasGroup>();
        if (imagenCanvasGroup == null)
            imagenCanvasGroup = imagenDialogo.gameObject.AddComponent<CanvasGroup>();

        movimientoJugador = FindObjectOfType<MovimientoJugador>();
        if (movimientoJugador != null)
            rbPlayer = movimientoJugador.GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(IniciarDialogoDespuesDeEspera());
    }

    void Update()
    {
        if (dialogoActivo && Input.GetKeyDown(KeyCode.E) && !esperandoInput)
        {
            StartCoroutine(DelayAntesDeSiguienteLinea()); // ✅ NUEVO
        }
    }

    System.Collections.IEnumerator DelayAntesDeSiguienteLinea()
    {
        esperandoInput = true;
        yield return new WaitForSeconds(2f); // ⏱️ DELAY DE 2 SEGUNDOS
        MostrarSiguienteLinea();
        esperandoInput = false;
    }

    System.Collections.IEnumerator IniciarDialogoDespuesDeEspera()
    {
        yield return new WaitForSeconds(1f);

        panelDialogo.SetActive(true);
        indiceLinea = 0;
        dialogoActivo = true;

        BloquearMovimiento(true);
        ActualizarDialogo();
    }

    void MostrarSiguienteLinea()
    {
        indiceLinea++;

        if (indiceLinea >= lineas.Length)
        {
            panelDialogo.SetActive(false);
            dialogoActivo = false;

            BloquearMovimiento(false);

            if (objetosADesaparecer != null)
            {
                for (int i = 0; i < objetosADesaparecer.Length; i++)
                    if (objetosADesaparecer[i] != null)
                        objetosADesaparecer[i].SetActive(false);
            }

            if (cameraDolly != null)
                cameraDolly.ForzarVuelta();
        }
        else
        {
            ActualizarDialogo();
        }
    }

    void BloquearMovimiento(bool bloquear)
    {
        if (movimientoJugador != null)
            movimientoJugador.SePuedeMover = !bloquear;

        if (rbPlayer != null && bloquear)
        {
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
        }
    }

    void ActualizarDialogo()
    {
        if (lineas == null || lineas.Length == 0)
        {
            textoDialogo.text = "";
            return;
        }

        if (mostrarTextoCoroutine != null)
            StopCoroutine(mostrarTextoCoroutine);

        mostrarTextoCoroutine = StartCoroutine(MostrarTextoPalabraPorPalabra(lineas[indiceLinea]));

        if (imagenes != null && indiceLinea < imagenes.Length && imagenes[indiceLinea] != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeImagen(imagenes[indiceLinea]));
        }
    }

    System.Collections.IEnumerator MostrarTextoPalabraPorPalabra(string linea)
    {
        textoDialogo.text = "";
        string[] palabras = linea.Split(' ');

        foreach (string palabra in palabras)
        {
            textoDialogo.text += palabra + " ";

            if (palabra.Contains("Bienvenido"))
                yield return StartCoroutine(ShakeTexto());

            yield return new WaitForSeconds(delayPalabra);
        }
    }

    System.Collections.IEnumerator ShakeTexto()
    {
        Vector3 posicionOriginal = textoDialogo.rectTransform.localPosition;
        float tiempo = 0f;

        while (tiempo < shakeDuracion)
        {
            float offsetX = Random.Range(-shakeIntensidad, shakeIntensidad);
            float offsetY = Random.Range(-shakeIntensidad, shakeIntensidad);
            textoDialogo.rectTransform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0);

            tiempo += Time.deltaTime;
            yield return null;
        }

        textoDialogo.rectTransform.localPosition = posicionOriginal;
    }

    System.Collections.IEnumerator FadeImagen(Sprite nuevaImagen)
    {
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duracionFade);
            yield return null;
        }

        imagenDialogo.sprite = nuevaImagen;

        t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duracionFade);
            yield return null;
        }
    }
}
