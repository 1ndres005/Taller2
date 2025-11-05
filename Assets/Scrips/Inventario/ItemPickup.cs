using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Datos del ítem")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject prefab3D;

    [Header("Objeto que aparecerá al recogerlo (opcional)")]
    public GameObject objetoAparecer; // Asignar en inspector (puede estar desactivado inicialmente)

    [Header("Contadores (visible en Inspector)")]
    [Tooltip("Cuántas veces se recogió este objeto (por instancia)")]
    public int pickupCount = 0; // contador por instancia
    [Tooltip("Contador global entre todas las instancias de ItemPickup")]
    public static int totalPickups = 0; // contador global estático

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Jugador puede recoger {itemName} (presiona E)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        // Si el jugador está cerca y presiona E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        // Añade al inventario si existe el sistema
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.AddItem(itemName, itemIcon, prefab3D);
        }

        // Aumenta contadores
        pickupCount += 1;       // cuenta para esta instancia
        totalPickups += 1;      // cuenta global

        // (Opcional) Guarda en PlayerPrefs para persistencia entre sesiones
        PlayerPrefs.SetInt(itemName + "_count", pickupCount);
        PlayerPrefs.SetInt("TotalPickups", totalPickups);
        PlayerPrefs.Save();

        Debug.Log($"{itemName} agregado al inventario. Contador de este ítem: {pickupCount}. Total recogidos: {totalPickups}");

        // Hace aparecer el nuevo objeto (si se asignó)
        if (objetoAparecer != null)
        {
            objetoAparecer.SetActive(true);
            Debug.Log($"{objetoAparecer.name} ha aparecido en la escena");
        }

        // Desactiva el objeto actual (el que se recogió)
        gameObject.SetActive(false);
    }

    // Método público para recuperar el conteo guardado (si lo necesitas)
    public void LoadSavedCount()
    {
        pickupCount = PlayerPrefs.GetInt(itemName + "_count", 0);
        // opcional: sincronizar totalPickups desde PlayerPrefs
        totalPickups = PlayerPrefs.GetInt("TotalPickups", totalPickups);
    }
}
