using UnityEngine;
using UnityEngine.UI;

public class VerificarObjetos : MonoBehaviour
{
    [Header("Referencia al ItemPickup (si está en escena)")]
    public ItemPickup itemPickup;

    [Header("Fallback / opciones")]
    public string itemNameFallback = "miItem";
    public bool usarContadorGlobal = false;

    [Header("Qué mostrar/activar")]
    public GameObject uiPanel;
    public GameObject objetoActivar;

    [Header("Slider de control")]
    public Slider sliderProgreso; // Arrastra aquí tu slider
    public float valorDesactivacion = 100f; // cuando llegue a este valor se desactiva el objeto

    private bool objetoActivo = false;

    private void Start()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
        if (objetoActivar != null) objetoActivar.SetActive(false);
    }

    private void Update()
    {
        // Si el slider llega al 100, desactiva el objeto
        if (sliderProgreso != null && sliderProgreso.value >= valorDesactivacion)
        {
            if (objetoActivar != null && objetoActivo)
            {
                objetoActivar.SetActive(false);
                objetoActivo = false; // permite volver a activarlo después
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int contador = ObtenerContador();

        if (contador <= 0)
        {
            if (uiPanel != null) uiPanel.SetActive(true);
        }
        else
        {
            if (objetoActivar != null)
            {
                objetoActivar.SetActive(true);
                objetoActivo = true;
            }

            if (uiPanel != null) uiPanel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (uiPanel != null) uiPanel.SetActive(false);
    }

    private int ObtenerContador()
    {
        if (itemPickup != null)
        {
            return itemPickup.pickupCount;
        }

        if (usarContadorGlobal)
        {
            return PlayerPrefs.GetInt("TotalPickups", 0);
        }
        else
        {
            return PlayerPrefs.GetInt(itemNameFallback + "_count", 0);
        }
    }
}
