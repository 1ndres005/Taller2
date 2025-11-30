using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [Header("Objeto en la mano")]
    public Transform manoTransform;   // Empty en la mano del jugador
    [HideInInspector] public GameObject objetoEnMano;

    // Recoger un objeto
    public void RecogerObjeto(GameObject objeto)
    {
        // Si ya tengo algo, no recojo otro
        if (objetoEnMano != null)
            return;

        objetoEnMano = objeto;

        // Desactivar física
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        Collider col = objeto.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        // Ponerlo en la mano
        objeto.transform.SetParent(manoTransform);
        objeto.transform.localPosition = Vector3.zero;
        objeto.transform.localRotation = Quaternion.identity;
    }

    // Soltar/colocar un objeto en un punto
    public void ColocarObjeto(Transform punto)
    {
        if (objetoEnMano == null)
            return;

        objetoEnMano.transform.SetParent(null);
        objetoEnMano.transform.position = punto.position;
        objetoEnMano.transform.rotation = punto.rotation;

        Rigidbody rb = objetoEnMano.GetComponent<Rigidbody>();
        Collider col = objetoEnMano.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        objetoEnMano = null;
    }
}
