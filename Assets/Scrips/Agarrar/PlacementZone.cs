using UnityEngine;

public class PlacementZone : MonoBehaviour
{
    public Transform puntoColocar; // Empty donde aparecerá el objeto
    public GameObject uiColocar;   // Texto "Pulsa P"

    bool jugadorCerca = false;
    PlayerItemHandler player;

    void Start()
    {
        player = FindObjectOfType<PlayerItemHandler>();

        if (uiColocar != null)
            uiColocar.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.P))
        {
            if (player.objetoEnMano != null)
            {
                player.ColocarObjeto(puntoColocar);

                if (uiColocar != null)
                    uiColocar.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (uiColocar != null)
                uiColocar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (uiColocar != null)
                uiColocar.SetActive(false);
        }
    }
}
