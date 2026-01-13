using UnityEngine;

public class TriggerKeyShowHideOnceAlt : MonoBehaviour
{
    public enum ActionType { ShowOnce, Hide }

    [Header("Objeto (flecha)")]
    public GameObject targetObject;

    [Header("Acción de ESTE trigger")]
    public ActionType action = ActionType.ShowOnce;

    [Header("Tecla")]
    public KeyCode key = KeyCode.R;

    [Header("UI opcional")]
    public GameObject uiPanel;

    // 🔒 Estado compartido (la flecha solo puede mostrarse UNA vez)
    private static bool hasShown = false;

    private bool playerInside = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(key))
        {
            if (targetObject == null) return;

            // 🔵 MOSTRAR SOLO UNA VEZ
            if (action == ActionType.ShowOnce)
            {
                if (hasShown) return;

                targetObject.SetActive(true);
                hasShown = true;
            }
            // 🔴 OCULTAR (sin límite)
            else if (action == ActionType.Hide)
            {
                targetObject.SetActive(false);
            }

            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (uiPanel != null)
        {
            // UI solo si el trigger puede usarse
            if (action == ActionType.ShowOnce && hasShown)
                uiPanel.SetActive(false);
            else
                uiPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }
}
