using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogoInicio_V2 : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public Image imagenDialogo;
    private CanvasGroup imagenCanvasGroup;

    [Header("Objeto que desaparece al terminar el diálogo")]
    public GameObject objetoADesaparecer; // 👈 arrastra aquí el objeto

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
    public Sprite[] imagenes;

    [Header("Configuración Fade")]
    public float duracionFade = 0.25f;

    [Header("Configuración Texto")]
    public float delayPalabra = 0.25f;
    public float shakeIntensidad = 5f;
    public float shakeDuracion = 0.3f;

    private int indiceLinea = 0;
    private bool dialogoActivo = false;
    private Coroutine fadeCoroutine;
    private Coroutine mostrarTextoCoroutine;

    void Awake()
    {
        imagenCanvasGroup = imagenDialogo.GetComponent<CanvasGroup>();
        if (imagenCanvasGroup == null)
            imagenCanvasGroup = imagenDialogo.gameObject.AddComponent<CanvasGroup>();
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
            // 🔚 Termina el diálogo
            panelDialogo.SetActive(false);
            dialogoActivo = false;

            // ❌ Desaparece el objeto elegido
            if (objetoADesaparecer != null)
                objetoADesaparecer.SetActive(false);
        }
        else
        {
            ActualizarDialogo();
        }
    }

    void ActualizarDialogo()
    {
        if (mostrarTextoCoroutine != null)
            StopCoroutine(mostrarTextoCoroutine);

        mostrarTextoCoroutine =
            StartCoroutine(MostrarTextoPalabraPorPalabra(lineas[indiceLinea]));

        if (imagenes != null && indiceLinea < imagenes.Length && imagenes[indiceLinea] != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

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
        Vector3 posOriginal = textoDialogo.rectTransform.localPosition;
        float tiempo = 0f;

        while (tiempo < shakeDuracion)
        {
            float x = Random.Range(-shakeIntensidad, shakeIntensidad);
            float y = Random.Range(-shakeIntensidad, shakeIntensidad);
            textoDialogo.rectTransform.localPosition =
                posOriginal + new Vector3(x, y, 0);

            tiempo += Time.deltaTime;
            yield return null;
        }

        textoDialogo.rectTransform.localPosition = posOriginal;
    }

    System.Collections.IEnumerator FadeImagen(Sprite nuevaImagen)
    {
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha =
                Mathf.Lerp(1f, 0f, t / duracionFade);
            yield return null;
        }

        imagenDialogo.sprite = nuevaImagen;

        t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            imagenCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, t / duracionFade);
            yield return null;
        }
    }
}
