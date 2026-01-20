using UnityEngine;
using TMPro;

public class PickupAndDisable : MonoBehaviour
{
    [Header("UI Counter")]
    public TextMeshProUGUI counterText;

    [Header("UI Interacción (Ej: 'Presiona M')")]
    public GameObject uiInteraccion; // 👈 UI que aparece al entrar al trigger

    [Header("Objects to Disable (Disappear)")]
    public GameObject[] objectsToDisable;

    [Header("Objects to Enable (Appear)")]
    public GameObject[] objectsToEnable;

    public static int counter = 0;

    private bool playerInside = false;

    void Start()
    {
        UpdateUI();

        if (uiInteraccion != null)
            uiInteraccion.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.M))
        {
            Pickup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            // 🟢 Mostrar UI
            if (uiInteraccion != null)
                uiInteraccion.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            // 🔴 Ocultar UI
            if (uiInteraccion != null)
                uiInteraccion.SetActive(false);
        }
    }

    void Pickup()
    {
        // 🔴 Ocultar UI
        if (uiInteraccion != null)
            uiInteraccion.SetActive(false);

        // Sumar contador
        counter++;
        UpdateUI();

        // Desactivar objetos
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Activar objetos
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Destruir este objeto
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        if (counterText != null)
            counterText.text = counter.ToString();
    }
}
