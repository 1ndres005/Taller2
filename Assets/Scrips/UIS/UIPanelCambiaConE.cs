using UnityEngine;

public class UIPanelCambiaConE : MonoBehaviour
{
    [Header("Pantallas de instrucciones en orden")]
    public GameObject[] pantallas;

    int indice = 0;
    bool terminado = false;

    void Start()
    {
        // Apagar todas menos la primera
        for (int i = 0; i < pantallas.Length; i++)
            pantallas[i].SetActive(i == 0);
    }

    void Update()
    {
        if (terminado)
            return;

        // Cambiar pantallas con E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Apagar pantalla actual
            pantallas[indice].SetActive(false);

            indice++;

            // Si llegó al final → cerrar todo
            if (indice >= pantallas.Length)
            {
                terminado = true;
                return;
            }

            // Activar la siguiente pantalla
            pantallas[indice].SetActive(true);
        }
    }
}
