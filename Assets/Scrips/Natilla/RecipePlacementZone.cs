using UnityEngine;

public class RecipePlacementZone : MonoBehaviour
{
    [Header("Puntos donde pueden ir los ingredientes")]
    public Transform[] puntosColocar;          // Array de puntos de colocación

    [Header("Tipos permitidos (uno por punto)")]
    public string[] tiposAceptados;            // Tipos requeridos por punto

    [Header("UI")]
    public GameObject uiColocar;               // Texto "Pulsa P"

    [Header("Receta")]
    public RecipeManager recipeManager;        // Gestor de la receta
    public bool soloUnaVezPorPunto = true;     // Cada punto solo se usa una vez

    bool jugadorCerca = false;
    bool[] puntosUsados;

    PlayerItemHandler player;

    void Start()
    {
        player = FindObjectOfType<PlayerItemHandler>();

        puntosUsados = new bool[puntosColocar.Length];

        if (uiColocar != null)
            uiColocar.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (player.objetoEnMano == null)
                return;

            GameObject obj = player.objetoEnMano;
            PickupItem pickup = obj.GetComponent<PickupItem>();

            if (pickup == null)
                return;

            int index = BuscarPuntoDisponible(pickup.tipoObjeto);

            if (index == -1)
            {
                Debug.Log("No hay un punto disponible para el tipo: " + pickup.tipoObjeto);
                return;
            }

            Transform punto = puntosColocar[index];
            player.ColocarObjeto(punto);

            if (soloUnaVezPorPunto)
                puntosUsados[index] = true;

            if (recipeManager != null)
                recipeManager.RegistrarIngredienteCorrecto();

            if (uiColocar != null)
                uiColocar.SetActive(false);
        }
    }

    int BuscarPuntoDisponible(string tipo)
    {
        for (int i = 0; i < puntosColocar.Length; i++)
        {
            if (i >= tiposAceptados.Length)
                break;

            if (tiposAceptados[i] == tipo && !puntosUsados[i])
                return i;
        }

        return -1;
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
