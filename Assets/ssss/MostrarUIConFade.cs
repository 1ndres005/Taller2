using UnityEngine;
using UnityEngine.UI;

public class MostrarUIConFade : MonoBehaviour
{
    [Header("Tiempo en segundos para mostrar la UI")]
    public float tiempoEspera = 120f;

    [Header("UI a mostrar")]
    public GameObject uiMostrar;

    [Header("Velocidad del parpadeo")]
    public float velocidadFade = 1f;

    private float tiempoActual = 0f;
    private bool mostrada = false;
    private bool estaParpadeando = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (uiMostrar != null)
        {
            uiMostrar.SetActive(false);

            // Agrega un CanvasGroup si no existe
            canvasGroup = uiMostrar.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = uiMostrar.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
        }
    }

    void Update()
    {
        if (uiMostrar == null) return;

        tiempoActual += Time.deltaTime;

        if (!mostrada && tiempoActual >= tiempoEspera)
        {
            uiMostrar.SetActive(true);
            canvasGroup.alpha = 1f;
            mostrada = true;
        }

        if (mostrada && Input.GetKeyDown(KeyCode.P) && !estaParpadeando)
        {
            StartCoroutine(Parpadear());
        }
    }

    System.Collections.IEnumerator Parpadear()
    {
        estaParpadeando = true;

        for (int i = 0; i < 3; i++) // Número de parpadeos
        {
            // Fade out
            yield return StartCoroutine(FadeCanvas(1f, 0f));
            // Fade in
            yield return StartCoroutine(FadeCanvas(0f, 1f));
        }

        estaParpadeando = false;
    }

    System.Collections.IEnumerator FadeCanvas(float desde, float hasta)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;
            canvasGroup.alpha = Mathf.Lerp(desde, hasta, t);
            yield return null;
        }
    }
}
