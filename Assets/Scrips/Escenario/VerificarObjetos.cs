using UnityEngine;

public class VerificarObjetos : MonoBehaviour
{
    [Header("Referencia al ItemPickup (si está en escena)")]
    public ItemPickup itemPickup; // arrastra la instancia del ItemPickup aquí (opcional)

    [Header("Fallback / opciones")]
    [Tooltip("Si itemPickup es null, usa esta clave para leer PlayerPrefs (itemName + \"_count\")")]
    public string itemNameFallback = "miItem";
    [Tooltip("Si true se usará el TotalPickups (contador global) en vez del contador por item")]
    public bool usarContadorGlobal = false;

    [Header("Qué mostrar/activar")]
    public GameObject uiPanel;         // Panel UI que se muestra cuando contador == 0
    public GameObject objetoActivar;   // Objeto que se activa cuando contador >= 1

    [Header("Comportamiento")]
    public bool activarSoloUnaVez = true; // si true, el objetoActivar se activa una sola vez
    private bool yaActivado = false;

    private void Start()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
        if (objetoActivar != null && activarSoloUnaVez) objetoActivar.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int contador = ObtenerContador();

        if (contador <= 0)
        {
            // mostrar UI si no tiene el item
            if (uiPanel != null) uiPanel.SetActive(true);
        }
        else // contador >= 1
        {
            // activar el objeto asignado (si corresponde)
            if (objetoActivar != null)
            {
                if (activarSoloUnaVez)
                {
                    if (!yaActivado)
                    {
                        objetoActivar.SetActive(true);
                        yaActivado = true;
                    }
                }
                else
                {
                    objetoActivar.SetActive(true);
                }
            }
            // asegurarse que la UI no esté visible
            if (uiPanel != null) uiPanel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ocultar la UI al salir; NO desactivar el objetoActivar para no romper el "persistir"
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    private int ObtenerContador()
    {
        // Si hay referencia al ItemPickup en escena, usar su pickupCount
        if (itemPickup != null)
        {
            return itemPickup.pickupCount;
        }

        // Si no hay referencia usamos PlayerPrefs: o contador global o por itemNameFallback
        if (usarContadorGlobal)
        {
            return PlayerPrefs.GetInt("TotalPickups", 0);
        }
        else
        {
            return PlayerPrefs.GetInt(itemNameFallback + "_count", 0);
        }
    }

    // Método público para forzar reiniciar la activación (útil para testing)
    public void ResetActivacion()
    {
        yaActivado = false;
        if (objetoActivar != null && activarSoloUnaVez) objetoActivar.SetActive(false);
    }
}
