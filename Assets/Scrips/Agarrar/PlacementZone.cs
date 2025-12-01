using UnityEngine;
using System.Linq; // para .Contains

public class PlacementZone : MonoBehaviour
{
    [Header("Colocación")]
    public Transform puntoColocar;   // Empty donde se posiciona el objeto

    [Header("UI")]
    public GameObject uiColocar;     // Texto "Pulsa P"

    [Header("Restricciones")]
    public string[] tiposAceptados;  // Ej: ["Leche"], ["Pan"], ["Leche","Carne"]
    public bool soloUnaVez = false;  // Si true, la zona se usa solo una vez

    bool jugadorCerca = false;
    bool usada = false;

    PlayerItemHandler player;

    void Start()
    {
        player = FindObjectOfType<PlayerItemHandler>();

        if (uiColocar != null)
            uiColocar.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca || usada)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (player.objetoEnMano == null)
                return;

            GameObject obj = player.objetoEnMano;
            PickupItem pickup = obj.GetComponent<PickupItem>();
            if (pickup == null)
                return;

            // ✅ Comprobamos si el tipo del objeto está en la lista de permitidos
            if (tiposAceptados != null && tiposAceptados.Length > 0)
            {
                if (!tiposAceptados.Contains(pickup.tipoObjeto))
                {
                    // Tipo NO permitido → aquí puedes poner un mensaje si quieres
                    Debug.Log("Este objeto no va en esta zona");
                    return;
                }
            }
            // Si la lista está vacía, la zona acepta cualquier objeto

            // Colocar el objeto
            player.ColocarObjeto(puntoColocar);

            if (soloUnaVez)
            {
                usada = true;
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;
            }

            if (uiColocar != null)
                uiColocar.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (usada) return;

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
