using UnityEngine;
using UnityEngine.UI;

public class SliderActivateObject : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;          // El slider

    [Header("Object to show")]
    public GameObject targetObject; // Objeto que aparecerá

    [Header("Options")]
    public bool disableSlider = true;

    bool activated = false;

    void Update()
    {
        if (activated) return;

        // Si el slider llegó a 1 (100%)
        if (slider != null && slider.value >= 1f)
        {
            activated = true;

            // Mostrar objeto
            if (targetObject != null)
                targetObject.SetActive(true);

            // Desactivar slider
            if (disableSlider)
                slider.gameObject.SetActive(false);
        }
    }
}
