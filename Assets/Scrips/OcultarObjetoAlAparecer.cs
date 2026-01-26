using UnityEngine;

public class OcultarObjetoAlAparecer : MonoBehaviour
{
    [Header("Objeto que se ocultará")]
    public GameObject objetoAOcultar;

    void OnEnable()
    {
        if (objetoAOcultar != null)
        {
            objetoAOcultar.SetActive(false);
        }
    }
}
