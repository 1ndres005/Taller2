
using UnityEngine;

public class ShowObjectWhenAnotherAppears : MonoBehaviour
{
    [Header("Objeto que se vigila")]
    public GameObject objectToWatch;   // Cuando este aparezca…

    [Header("Objeto que aparece")]
    public GameObject objectToShow;    // …este se activa

    private bool activated = false;

    void Start()
    {
        if (objectToShow != null)
            objectToShow.SetActive(false);
    }

    void Update()
    {
        if (activated) return;
        if (objectToWatch == null || objectToShow == null) return;

        // Si el objeto vigilado está activo
        if (objectToWatch.activeInHierarchy)
        {
            objectToShow.SetActive(true);
            activated = true; // Solo una vez
        }
    }
}
