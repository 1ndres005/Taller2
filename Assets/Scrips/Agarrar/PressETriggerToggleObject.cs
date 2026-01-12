using UnityEngine;

public class PressETriggerToggleObject : MonoBehaviour
{
    public enum ActionType { Show, Hide }

    [Header("Objeto a mostrar/ocultar (misma flecha)")]
    public GameObject targetObject;

    [Header("Acción de ESTE trigger")]
    public ActionType action = ActionType.Show;

    [Header("Tecla")]
    public KeyCode interactKey = KeyCode.E;

    [Header("UI opcional")]
    public GameObject uiPanel;

    [Header("Configuración SHOW")]
    [Tooltip("Número máximo de veces que se puede MOSTRAR el objeto (ej: 3, 5, 10)")]
    public int maxShows = 3;

    [Tooltip("Cada trigger SHOW solo se puede usar una vez")]
    public bool showOncePerTrigger = true;

    [Header("Configuración HIDE")]
    [Tooltip("Si está activo, el HIDE solo puede usarse una vez en total")]
    public bool hideOnlyOnce = false;

    // 🔒 ESTADO COMPARTIDO ENTRE TODOS LOS TRIGGERS
    private static int currentShowCount = 0;
    private static bool hideUsed = false;

    // 🔒 ESTADO LOCAL DEL TRIGGER
    private bool playerInside = false;
    private bool usedThisTrigger = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        // Bloqueo si este trigger ya se usó
        if (showOncePerTrigger && usedThisTrigger)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            if (targetObject == null) return;

            if (action == ActionType.Show)
            {
                // No permitir más de maxShows
                if (currentShowCount >= maxShows)
                    return;

                targetObject.SetActive(true);
                currentShowCount++;
                usedThisTrigger = true;
            }
            else // HIDE
            {
                if (hideOnlyOnce && hideUsed)
                    return;

                targetObject.SetActive(false);
                hideUsed = true;
                usedThisTrigger = true;
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
            bool canUse = true;

            if (action == ActionType.Show)
            {
                if (currentShowCount >= maxShows) canUse = false;
                if (showOncePerTrigger && usedThisTrigger) canUse = false;
            }
            else // Hide
            {
                if (hideOnlyOnce && hideUsed) canUse = false;
            }

            uiPanel.SetActive(canUse);
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
