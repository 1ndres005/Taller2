using UnityEngine;
using System;

public class TriggerInteractR : MonoBehaviour
{
    public KeyCode key = KeyCode.R;
    public GameObject uiPanel; // opcional: "Presiona R"

    public event Action onInteract;

    private bool playerInside = false;

    void Start()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(key))
        {
            onInteract?.Invoke();
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
