using UnityEngine;
using System.Collections;

public class BlinkPressAnyKey : MonoBehaviour
{
    [Header("UI Principal (parpadea)")]
    public CanvasGroup canvasGroup;
    public RectTransform targetScale; // opcional

    [Header("Objeto Secundario (Background)")]
    public CanvasGroup canvasGroupExtra;
    public float delayExtraFade = 0f;

    [Header("Objeto Tercero (UI extra - solo fade)")]
    public CanvasGroup canvasGroupTercero;
    public float delayTerceroFade = 0f; // X segundos DESPUÉS del fade del background

    [Header("Delay inicial")]
    public float delayAntesDeAparecer = 2f;

    [Header("Parpadeo")]
    [Range(0f, 1f)] public float minAlpha = 0.15f;
    [Range(0f, 1f)] public float maxAlphaMientrasEspera = 0.75f;
    public float blinkSpeed = 2.5f;

    [Header("Confirmación")]
    public float fadeToOneDuration = 0.25f;
    public float delayAntesDeDesvanecer = 1.5f;
    public float fadeOutDuration = 0.5f;

    [Header("Feedback extra")]
    public bool hacerPop = true;
    public float popScale = 1.08f;
    public float popDuration = 0.12f;

    private bool esperandoInput = false;
    private bool yaConfirmado = false;
    private Coroutine blinkRoutine;

    void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        targetScale = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // principal inicia invisible
        canvasGroup.alpha = 0f;
        esperandoInput = false;
        yaConfirmado = false;

        // background y tercero por defecto visibles (si existen)
        if (canvasGroupExtra != null) canvasGroupExtra.alpha = 1f;
        if (canvasGroupTercero != null) canvasGroupTercero.alpha = 1f;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        StartCoroutine(DelayInicio());
    }

    IEnumerator DelayInicio()
    {
        yield return new WaitForSecondsRealtime(delayAntesDeAparecer);

        esperandoInput = true;
        canvasGroup.alpha = maxAlphaMientrasEspera;
        blinkRoutine = StartCoroutine(BlinkLoop());
    }

    void Update()
    {
        if (!esperandoInput || yaConfirmado) return;

        if (Input.anyKeyDown ||
            Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1) ||
            Input.GetMouseButtonDown(2))
        {
            Confirmar();
        }
    }

    IEnumerator BlinkLoop()
    {
        while (esperandoInput)
        {
            float t = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);
            canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlphaMientrasEspera, t);
            yield return null;
        }
    }

    void Confirmar()
    {
        yaConfirmado = true;
        esperandoInput = false;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        if (hacerPop && targetScale != null)
            StartCoroutine(Pop());

        StartCoroutine(ConfirmFlow());
    }

    IEnumerator ConfirmFlow()
    {
        // Fade a 1 (principal)
        yield return FadeTo(canvasGroup, 1f, fadeToOneDuration);

        // Espera antes de desaparecer
        yield return new WaitForSecondsRealtime(delayAntesDeDesvanecer);

        // Fade out principal (en paralelo)
        StartCoroutine(FadeTo(canvasGroup, 0f, fadeOutDuration));

        // Fade out background y luego el tercero (encadenado)
        if (canvasGroupExtra != null)
            StartCoroutine(FadeExtraYLuegoTercero());
        else
        {
            // Si no hay background, igual respeta el delay del tercero desde "ya"
            if (canvasGroupTercero != null)
                StartCoroutine(FadeTerceroDespues(delayTerceroFade));
        }
    }

    IEnumerator FadeExtraYLuegoTercero()
    {
        // Delay antes de empezar background
        yield return new WaitForSecondsRealtime(delayExtraFade);

        // Fade out background y esperar a que termine
        yield return FadeTo(canvasGroupExtra, 0f, fadeOutDuration);

        // Luego esperar X segundos y desvanecer el tercero
        if (canvasGroupTercero != null)
        {
            yield return new WaitForSecondsRealtime(delayTerceroFade);
            yield return FadeTo(canvasGroupTercero, 0f, fadeOutDuration);
        }
    }

    IEnumerator FadeTerceroDespues(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        yield return FadeTo(canvasGroupTercero, 0f, fadeOutDuration);
    }

    IEnumerator FadeTo(CanvasGroup cg, float target, float duration)
    {
        float start = cg.alpha;
        float t = 0f;

        if (duration <= 0f)
        {
            cg.alpha = target;
            yield break;
        }

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        cg.alpha = target;
    }

    IEnumerator Pop()
    {
        Vector3 baseScale = targetScale.localScale;
        Vector3 up = baseScale * popScale;

        float t = 0f;
        while (t < popDuration)
        {
            t += Time.unscaledDeltaTime;
            targetScale.localScale = Vector3.Lerp(baseScale, up, t / popDuration);
            yield return null;
        }

        t = 0f;
        while (t < popDuration)
        {
            t += Time.unscaledDeltaTime;
            targetScale.localScale = Vector3.Lerp(up, baseScale, t / popDuration);
            yield return null;
        }

        targetScale.localScale = baseScale;
    }
}
