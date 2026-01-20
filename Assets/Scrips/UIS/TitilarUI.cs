using UnityEngine;
using System.Collections;

public class TitilarUI : MonoBehaviour
{
    [Header("Configuración del Titileo")]
    [Tooltip("Cada cuántos segundos se repite el titileo")]
    public float intervalo = 3f;

    [Tooltip("Cuántas veces titila en cada intervalo")]
    public int repeticiones = 2;

    [Tooltip("Duración de cada titileo (ida o vuelta)")]
    public float duracionTitileo = 0.15f;

    [Tooltip("Qué tan fuerte titila (0 = desaparece, 1 = no se nota)")]
    [Range(0f, 1f)]
    public float intensidad = 0.3f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StartCoroutine(TitilarLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 1f;
    }

    IEnumerator TitilarLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            for (int i = 0; i < repeticiones; i++)
            {
                // Baja opacidad
                yield return StartCoroutine(Fade(1f, intensidad));

                // Sube opacidad
                yield return StartCoroutine(Fade(intensidad, 1f));
            }
        }
    }

    IEnumerator Fade(float desde, float hasta)
    {
        float t = 0f;

        while (t < duracionTitileo)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(desde, hasta, t / duracionTitileo);
            yield return null;
        }

        canvasGroup.alpha = hasta;
    }
}
