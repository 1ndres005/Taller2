using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogoInicio : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public Image imagenDialogo;   // La imagen de UI que quieres cambiar
    private CanvasGroup imagenCanvasGroup; // Para el fade

    [Header("Objetos a desaparecer al terminar el diálogo")]
    public GameObject[] objetosADesaparecer; // 👈 arrastra aquí varios objetos

    [Header("Contenido del diálogo")]
    [TextArea(3, 6)]
    private string[] lineas = {
        "Bienvenido a Lo mero paisa, un rinconcito lleno de tradición y alegría",
        "Aquí usted será el protagonista de su propia historia, con muchas aventuras por descubrir.",
        "Nuestro pueblito tiene gente muy trabajadora que siempre necesita una mano amiga.",
        "Le espera una jornada especial, llena de sabores, música y la calidez de nuestra tierra.",
        "Por ahora, hay varios caminos que puede tomar, y la decisión es suya.",
        "Puede ayudar a don Iván en su jardín, preparando una silleta para la próxima feria.",
        "También puede visitar a Sofía junto al río, que anda cocinando un delicioso sancocho.",
        "O si prefiere la montaña, Juan está recogiendo café cerca del cementerio y necesita compañía.",
        "¿Entonces qué dice? ¿Por dónde quiere empezar esta aventura?"
    };

    [Header("Imágenes para cada línea")]
    public Sprite[] imagenes; // Debe tener la misma cantidad de elementos que "lineas"

    [Header("Configuración Fade")]
    public float duracionFade = 0.25f; // duración del fade (en segundos)

    [Header("Configuración Texto")]
    public float delayPalabra = 0.25f; // tiempo entre cada palabra
    public float shakeIntensidad = 5f; // intensidad de vibración
    public float shakeDuracion = 0.3f; // duración de vibración

    private int indiceLinea = 0;
    private bool dialogoActivo = false;
    private Coroutine fadeCoroutine;
    private Coroutine mostrarTextoCoroutine;

    // ✅ Referencia al PlayerMovement
    private PlayerMovement playerMovement;

    void Awake()
    {
        // Asegurarse de que la imagen tiene un CanvasGroup
        imagenCanvasGroup = imagenDialogo.GetComponent<CanvasGroup>();
        if (imagenCanvasGroup == null)
        {
            imagenCanvasGroup = imagenDialogo.gameObject.AddComponent<CanvasGroup>();
        }

        // Buscar automáticamente el PlayerMovement en la escena
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        StartCoroutine(IniciarDialogoDespuesDeEspera());
    }

    void Update()
    {
        if (dialogoActivo && Input.GetKeyDown(KeyCode.E))
        {
            MostrarSiguienteLinea();
        }
    }

    System.Collections.IEnumerator IniciarDialogoDespuesDeEspera()
    {
        yield return new WaitForSeconds(1f);
        panelDialogo.SetActive(true);
        indiceLinea = 0;
        ActualizarDialogo();
        dialogoActivo = true;
    }

    void MostrarSiguienteLinea()
    {
        indiceLinea++;

        if (indiceLinea >= lineas.Length)
        {
            panelDialogo.SetActive(false);
            dialogoActivo = false;

            // ✅ Al terminar el diálogo, desaparecer todos los objetos del array
            if (objetosADesaparecer != null)
            {
                for (int i = 0; i < objetosADesaparecer.Length; i++)
                {
                    if (objetosADesaparecer[i] != null)
                        objetosADesaparecer[i].SetActive(false);
                }
            }
        }
        else
        {
            ActualizarDialogo();
        }
    }

    void ActualizarDialogo()
    {
        // Detener corutina de texto si estaba corriendo
        if (mostrarTextoCoroutine != null)
            StopCoroutine(mostrarTextoCoroutine);

        mostrarTextoCoroutine = StartCoroutine(MostrarTextoPalabraPorPalabra(lineas[indiceLinea]));

        // Cambiar imagen con fade
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

            // Si la palabra contiene "Bienvenido", aplicar vibración
            if (palabra.Contains("Bienvenido"))
            {
                yield return StartCoroutine(ShakeTexto());
            }

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
        // Fade out
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duracionFade);
            yield return null;
        }

        // Cambiar sprite
        imagenDialogo.sprite = nuevaImagen;

        // Fade in
        t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duracionFade);
            yield return null;
        }
    }
}
