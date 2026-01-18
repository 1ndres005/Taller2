using UnityEngine;

public class MostrarYOcultarAlAparecer : MonoBehaviour
{
    [Header("Objetos a controlar")]
    public GameObject objetoAMostrar;
    public GameObject objetoAOcultar;

    void OnEnable()
    {
        if (objetoAMostrar != null)
            objetoAMostrar.SetActive(true);

        if (objetoAOcultar != null)
            objetoAOcultar.SetActive(false);
    }
}
