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

        // Desactivar física mientras está en la mano
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        Collider col = objeto.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        // Ponerlo en la mano
        objeto.transform.SetParent(manoTransform);
        objeto.transform.localPosition = Vector3.zero;
        objeto.transform.localRotation = Quaternion.identity;
    }

    // Colocar un objeto en un punto del mundo (mesa, zona, etc.)
    public void ColocarObjeto(Transform punto)
    {
        if (objetoEnMano == null)
            return;

        GameObject obj = objetoEnMano;

        // Quitar de la mano
        obj.transform.SetParent(null);
        obj.transform.position = punto.position;
        obj.transform.rotation = punto.rotation;  // o quita esta línea si te giraba raro

        // Activar física de nuevo
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Collider col = obj.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        // ❗ Desactivar que se pueda volver a recoger
        PickupItem pickup = obj.GetComponent<PickupItem>();
        if (pickup != null)
        {
            pickup.DesactivarPickup();
        }

        objetoEnMano = null;
    }
}
