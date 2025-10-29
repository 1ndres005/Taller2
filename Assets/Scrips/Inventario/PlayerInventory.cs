using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI; // <-- Agrega esta directiva para usar 'Image'

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private void Awake() => Instance = this;

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public Sprite itemIcon;
        public GameObject prefab3D; // <-- Agrega este campo para almacenar el prefab 3D
    }

    public List<InventoryItem> items = new List<InventoryItem>();

    [Header("UI")]
    public GameObject InventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private readonly bool inventoryPanel = false; // <-- 'readonly' agregado

    private bool inventoryOpen = false;

    public void AddItem(string name, Sprite icon, GameObject itemInspectPrefab)
    {
        InventoryItem newItem = new InventoryItem
        {
            itemName = name,
            itemIcon = icon,
            prefab3D = itemInspectPrefab // <-- ahora sí lo guardas
        };

        items.Add(newItem);
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            slot.GetComponent<Image>().sprite = item.itemIcon;

            // cuando hagas clic, inspeccionas el objeto
            Button btn = slot.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                ItemInspectorManager.Instance.ShowItem(item.prefab3D);
            });
        }
    }

    private void Update()
    {
        if (GameInput.Instance != null && GameInput.Instance.InventoryPressed())
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        InventoryPanel.SetActive(inventoryOpen);
    }
}


