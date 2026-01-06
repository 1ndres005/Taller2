using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingTextDots : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 0.4f;

    void Start()
    {
        if (!text)
            text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        string baseText = "Cargando";
        int dots = 0;

        while (true)
        {
            dots = (dots + 1) % 4; // 0..3
            text.text = baseText + new string('.', dots);
            yield return new WaitForSeconds(speed);
        }
    }
}
