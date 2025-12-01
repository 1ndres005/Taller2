using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiInteract;              // Texto "Pulsa R"

    [Header("Comportamiento")]
    public bool mantenerCollider = false;      // Si true, no se desactiva el collider (plataformas)
    public bool sePuedeMoverDespues = false;   // Si true, se podrá volver a recoger después de colocarlo

    [Header("Tipo de objeto (para zonas específicas)")]
    public string tipoObjeto;                  // Ej: "Leche", "Olla", "Pan"

    [HideInInspector] public Quaternion rotacionOriginal;

    bool jugadorCerca = false;
    bool puedeRecogerse = true;

    PlayerItemHandler player;

    void Start()
    {
        // Guardar la rotación con la que estaba en la escena
        rotacionOriginal = transform.rotation;

        player = FindObjectOfType<PlayerItemHandler>();

        if (uiInteract != null)
            uiInteract.SetActive(false);
    }

    void Update()
    {
        if (!puedeRecogerse)
            return;

        // Recoger con R
        if (jugadorCerca && Input.GetKeyDown(KeyCode.R))
        {
            if (player != null && player.objetoEnMano == null)
            {
                player.RecogerObjeto(gameObject);

                if (uiInteract != null)
                    uiInteract.SetActive(false);
            }
        }
    }

    public void DesactivarPickup()
    {
        // Si NO quieres que se pueda mover otra vez
        if (!sePuedeMoverDespues)
        {
            puedeRecogerse = false;
            jugadorCerca = false;

            if (uiInteract != null)
                uiInteract.SetActive(false);

            // Si no es plataforma, se puede apagar el collider
            if (!mantenerCollider)
            {
                Collider col = GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
            }
        }
        // Si sePuedeMoverDespues == true, no tocamos nada:
        // el objeto se puede volver a agarrar más tarde.
    }

    void OnTriggerEnter(Collider other)
    {
        if (!puedeRecogerse) return;

        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (uiInteract != null && player != null && player.objetoEnMano == null)
                uiInteract.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!puedeRecogerse) return;

        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            if (uiInteract != null)
                uiInteract.SetActive(false);
        }
    }
}
