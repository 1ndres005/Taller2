using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [Header("Objeto en la mano")]
    public Transform manoTransform;          // Empty en la mano o frente a la cámara
    [HideInInspector] public GameObject objetoEnMano;

    // Recoger un objeto
    public void RecogerObjeto(GameObject objeto)
    {
        if (objetoEnMano != null)
            return; // ya tengo algo en la mano

        objetoEnMano = objeto;

        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        Collider col = objeto.GetComponent<Collider>();
        PickupItem pickup = objeto.GetComponent<PickupItem>();

        // Desactivar físicas mientras está en la mano
        if (rb != null) rb.isKinematic = true;

        // Si NO es objeto especial de plataforma, desactivamos su collider
        if (col != null && pickup != null && !pickup.mantenerCollider)
            col.enabled = false;

        // Ponerlo como hijo de la mano
        objeto.transform.SetParent(manoTransform);

        // Posición fija en la mano
        objeto.transform.localPosition = Vector3.zero;

        // ⭐ Mantener rotación ORIGINAL con la que estaba en la escena
        if (pickup != null)
            objeto.transform.rotation = pickup.rotacionOriginal;
    }

    // Colocar un objeto en un punto del mundo (mesa, zona, etc.)
    public void ColocarObjeto(Transform punto)
    {
        if (objetoEnMano == null)
            return;

        GameObject obj = objetoEnMano;
        PickupItem pickup = obj.GetComponent<PickupItem>();

        // Soltar del jugador
        obj.transform.SetParent(null);

        // Posición donde lo colocas
        obj.transform.position = punto.position;

        // ⭐ Mantener su rotación original también al colocarlo
        if (pickup != null)
            obj.transform.rotation = pickup.rotacionOriginal;

        // Volver a activar físicas
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Collider col = obj.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        // Avisar al objeto que ya fue colocado (según flags)
        if (pickup != null)
            pickup.DesactivarPickup();

        objetoEnMano = null;
    }
}
