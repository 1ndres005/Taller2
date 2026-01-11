using UnityEngine;

public class PickupRestoreMaterial : MonoBehaviour
{
    [Header("Renderer")]
    [Tooltip("Si se deja vacío, usa el Renderer de este objeto.")]
    public Renderer targetRenderer;

    [Header("Material Normal")]
    [Tooltip("Material normal al que debe volver el objeto.")]
    public Material normalMaterial;

    [Header("Opciones")]
    public bool disableColliderAfterPickup = true;

    void Awake()
    {
        // Si no asignas renderer, usa el propio
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (targetRenderer != null && normalMaterial != null)
        {
            // 🔥 Volver al material normal
            targetRenderer.material = normalMaterial;
        }

        // Opcional: desactivar collider para que no vuelva a activarse
        if (disableColliderAfterPickup)
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }
}
