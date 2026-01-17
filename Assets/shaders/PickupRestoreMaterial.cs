using UnityEngine;

public class PickupRestoreMaterial : MonoBehaviour
{
    [Header("Renderer")]
    [Tooltip("Si se deja vacío, usa el Renderer de este objeto.")]
    public Renderer targetRenderer;

    [Header("Material Normal")]
    [Tooltip("Material normal al que debe volver el objeto.")]
    public Material normalMaterial;

    [Header("Interacción")]
    public KeyCode interactKey = KeyCode.E; // ⬅️ tecla que tú quieras

    [Header("Opciones")]
    public bool disableColliderAfterPickup = true;

    private bool playerInside = false;
    private bool used = false;

    void Awake()
    {
        // Si no asignas renderer, usa el propio
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!playerInside || used)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            RestoreMaterial();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
    }

    void RestoreMaterial()
    {
        if (targetRenderer != null && normalMaterial != null)
        {
            // 🔄 Volver al material normal
            targetRenderer.material = normalMaterial;
        }

        used = true;

        // Opcional: desactivar collider para que no vuelva a activarse
        if (disableColliderAfterPickup)
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }
}
