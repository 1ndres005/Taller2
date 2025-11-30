using UnityEngine;
using System.Collections;

public class PuertaNeveraInteractiva : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Objeto PIVOT que rota (empty en la bisagra)")]
    public Transform puertaPivot;       // El empty que actúa como bisagra

    [Tooltip("UI que muestra 'Pulsa E'")]
    public GameObject uiInteract;       // Texto de interacción (Pulsa E)

    [Header("Rotaciones (en grados, local)")]
    [Tooltip("Rotación cuando la puerta está CERRADA")]
    public Vector3 rotacionCerradaEuler;    // Ej: (0, 0, 0)

    [Tooltip("Rotación cuando la puerta está ABIERTA")]
    public Vector3 rotacionAbiertaEuler;    // Ej: (0, 90, 0) o (0, -90, 0)

    [Header("Animación")]
    public float tiempoAnimacion = 0.3f;    // Qué tan rápido abre/cierra

    bool jugadorCerca = false;
    bool abierta = false;
    bool animando = false;

    void Start()
    {
        if (puertaPivot == null)
            puertaPivot = transform;

        // Al inicio, forzamos la puerta a la rotación CERRADA
        puertaPivot.localRotation = Quaternion.Euler(rotacionCerradaEuler);

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        // Solo permitir interacción si el jugador está cerca y no está animando
        if (!jugadorCerca || animando)
            return;

        // Cambiar estado con E
        if (Input.GetKeyDown(KeyCode.E))
        {
            abierta = !abierta;

            Quaternion desde = puertaPivot.localRotation;
            Quaternion hacia = abierta
                ? Quaternion.Euler(rotacionAbiertaEuler)
                : Quaternion.Euler(rotacionCerradaEuler);

            StartCoroutine(RotarPuerta(desde, hacia));
        }
    }

    IEnumerator RotarPuerta(Quaternion desde, Quaternion hacia)
    {
        animando = true;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / tiempoAnimacion;
            puertaPivot.localRotation = Quaternion.Slerp(desde, hacia, t);
            yield return null;
        }

        puertaPivot.localRotation = hacia;
        animando = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (uiInteract != null)
                uiInteract.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (uiInteract != null)
                uiInteract.SetActive(false);
        }
    }
}
