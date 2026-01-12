using UnityEngine;

public class PressETriggerToggleObject : MonoBehaviour
{
    public enum ActionType { Show, Hide }

    [Header("Objeto a mostrar/ocultar (el MISMO en ambos triggers)")]
    public GameObject targetObject;

    [Header("Acción de ESTE trigger")]
    public ActionType action = ActionType.Show;

    [Header("Tecla")]
    public KeyCode interactKey = KeyCode.E;

    [Header("UI opcional (Presiona E)")]
    public GameObject uiPanel;

    private bool playerInside;

    void Start()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (targetObject != null)
            {
                bool state = (action == ActionType.Show);
                targetObject.SetActive(state);
            }

            if (uiPanel != null) uiPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        if (uiPanel != null) uiPanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (uiPanel != null) uiPanel.SetActive(false);
    }
}
