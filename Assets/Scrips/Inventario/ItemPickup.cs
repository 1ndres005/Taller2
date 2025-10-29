using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject prefab3D;

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
        // Usando el nuevo Input System
        if (isPlayerInRange && GameInput.Instance != null && GameInput.Instance.InteractPressed())
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        PlayerInventory.Instance.AddItem(itemName, itemIcon, prefab3D);
        Debug.Log($"{itemName} agregado al inventario");
        gameObject.SetActive(false); // ahora sí se desactiva solo al presionar E
    }
}
