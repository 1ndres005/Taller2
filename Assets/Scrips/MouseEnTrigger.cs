using UnityEngine;

public class MouseEnTrigger : MonoBehaviour
{
    [Header("Opciones")]
    public bool bloquearMovimiento = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (bloquearMovimiento)
                Time.timeScale = 0f; // opcional
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (bloquearMovimiento)
                Time.timeScale = 1f; // opcional
        }
    }
}
