using System.Collections;
using UnityEngine;

public class ToggleFuegoConTrigger : MonoBehaviour
{
    [Header("UI al entrar (se muestra por X segundos)")]
    public GameObject enterUIPanel;
    public float uiShowTime = 2f;

    [Header("Slider (GameObject raíz del slider o contenedor)")]
    public GameObject sliderRoot;

    [Header("Partículas (fuego)")]
    public ParticleSystem fuegoParticles;

    [Header("Tecla")]
    public KeyCode toggleKey = KeyCode.Q;

    private bool jugadorDentro = false;
    private bool fuegoEncendido = false;
    private Coroutine uiRoutine;

    void Start()
    {
        // Estado inicial
        if (enterUIPanel != null) enterUIPanel.SetActive(false);
        if (sliderRoot != null) sliderRoot.SetActive(false);

        if (fuegoParticles != null)
        {
            fuegoParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            fuegoParticles.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Solo permite usar Q si el jugador está dentro del trigger
        if (!jugadorDentro) return;

        if (Input.GetKeyDown(toggleKey))
        {
            fuegoEncendido = !fuegoEncendido;

            if (fuegoEncendido)
                TurnOn();
            else
                TurnOff();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorDentro = true;

        // Mostrar UI por X segundos
        if (enterUIPanel != null)
        {
            if (uiRoutine != null) StopCoroutine(uiRoutine);
            uiRoutine = StartCoroutine(ShowUIForSeconds(uiShowTime));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorDentro = false;

        // Si sale del trigger, se apaga TODO
        fuegoEncendido = false;
        TurnOff();

        // Ocultar UI inmediatamente al salir
        if (enterUIPanel != null)
            enterUIPanel.SetActive(false);
    }

    void TurnOn()
    {
        // Mostrar slider
        if (sliderRoot != null)
            sliderRoot.SetActive(true);

        // Activar partículas
        if (fuegoParticles != null)
        {
            fuegoParticles.gameObject.SetActive(true);
            fuegoParticles.Play(true);
        }
    }

    void TurnOff()
    {
        // Ocultar slider
        if (sliderRoot != null)
            sliderRoot.SetActive(false);

        // Apagar partículas
        if (fuegoParticles != null)
        {
            fuegoParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            fuegoParticles.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowUIForSeconds(float seconds)
    {
        enterUIPanel.SetActive(true);
        yield return new WaitForSeconds(seconds);
        enterUIPanel.SetActive(false);
    }
}
