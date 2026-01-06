using UnityEngine;
using TMPro;

public class PickupAndDisable : MonoBehaviour
{
    [Header("UI Counter")]
    public TextMeshProUGUI counterText;

    [Header("Object to Disable")]
    public GameObject objectToDisable; // El otro objeto que quieres apagar

    public static int counter = 0;

    void Start()
    {
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Sumar contador
            counter++;
            UpdateUI();

            // Desactivar el objeto elegido
            if (objectToDisable != null)
                objectToDisable.SetActive(false);

            // Desaparecer este objeto
            Destroy(gameObject);
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
            counterText.text = counter.ToString();
    }
}
