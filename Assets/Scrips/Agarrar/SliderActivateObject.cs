using UnityEngine;
using UnityEngine.UI;

public class SliderActivateObject : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;   // Slider (0 → 1)

    [Header("Objects to HIDE")]
    public GameObject[] objectsToHide;   // Objetos que desaparecen

    [Header("Objects to SHOW")]
    public GameObject[] objectsToShow;   // Objetos que aparecen

    [Header("Options")]
    public bool disableSlider = true;

    private bool activated = false;

    void Update()
    {
        if (activated) return;
        if (slider == null) return;

        // Cuando el slider llega a 1 (100%)
        if (slider.value >= 1f)
        {
            activated = true;

            // ❌ Ocultar objetos
            if (objectsToHide != null)
            {
                foreach (GameObject obj in objectsToHide)
                {
                    if (obj != null)
                        obj.SetActive(false);
                }
            }

            // ✅ Mostrar objetos
            if (objectsToShow != null)
            {
                foreach (GameObject obj in objectsToShow)
                {
                    if (obj != null)
                        obj.SetActive(true);
                }
            }

            // Opcional: ocultar el slider
            if (disableSlider)
                slider.gameObject.SetActive(false);
        }
    }
}
