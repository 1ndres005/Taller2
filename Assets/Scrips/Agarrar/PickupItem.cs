using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public GameObject uiInteract;  // Texto "Pulsa E"
    bool jugadorCerca = false;
    PlayerItemHandler player;

    void Start()
    {
        player = FindObjectOfType<PlayerItemHandler>();

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
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
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (uiInteract != null && player.objetoEnMano == null)
                uiInteract.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (uiInteract != null)
                uiInteract.SetActive(false);
        }
    }
}
