using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public GameObject uiInteract;  // Texto "Pulsa E"
    bool jugadorCerca = false;
    bool puedeRecogerse = true;

    PlayerItemHandler player;

    void Start()
    {
        player = FindObjectOfType<PlayerItemHandler>();

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        if (!puedeRecogerse)
            return; // ya no se puede recoger

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (player.objetoEnMano == null)  // Solo si no tiene otro
            {
                player.RecogerObjeto(gameObject);
                if (uiInteract != null)
                    uiInteract.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!puedeRecogerse)
            return;

        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (uiInteract != null && player.objetoEnMano == null)
                uiInteract.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!puedeRecogerse)
            return;

        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (uiInteract != null)
                uiInteract.SetActive(false);
        }
    }

    // 👉 Llamado cuando el objeto se coloca en una zona
    public void DesactivarPickup()
    {
        puedeRecogerse = false;
        jugadorCerca = false;

        if (uiInteract != null)
            uiInteract.SetActive(false);

        // Opcional: desactivar el collider para que no moleste más
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
}
