using UnityEngine;

public class TriggerToggleMouse : MonoBehaviour
{
    [Header("Tecla para activar el mouse")]
    public KeyCode activateKey = KeyCode.E;

    private bool playerInside = false;

    void Update()
    {
        // Solo permite usar E si el jugador está dentro del trigger
        if (!playerInside) return;

        if (Input.GetKeyDown(activateKey))
        {
            ActivateMouse();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        DeactivateMouse();
    }

    void ActivateMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void DeactivateMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
