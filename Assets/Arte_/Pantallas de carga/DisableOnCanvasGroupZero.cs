using UnityEngine;

public class DisableOnCanvasGroupZero : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject object1;
    public GameObject object2;

    private bool alreadyDisabled = false;

    void Update()
    {
        if (!alreadyDisabled && canvasGroup.alpha == 0f)
        {
            object1.SetActive(false);
            object2.SetActive(false);
            alreadyDisabled = true;
        }
    }
}
