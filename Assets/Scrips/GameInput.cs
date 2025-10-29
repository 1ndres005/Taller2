using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    private PlayerController controls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            controls = new PlayerController();
            controls.Gameplay.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // === Lectura de Inputs ===
    public bool InteractPressed()
    {
        return controls.Gameplay.Interaction.WasPressedThisFrame();
    }

    public bool InventoryPressed()
    {
        return controls.Gameplay.Inventory.WasPressedThisFrame();
    }
    public Vector2 GetMouseDelta()
    {
        // Corregido: Acceder a Look a través de Gameplay
        return controls.Gameplay.Look.ReadValue<Vector2>();
    }
    public bool CancelPressed()
    {
        return controls.Gameplay.Cancel.WasPressedThisFrame();
    }
}
