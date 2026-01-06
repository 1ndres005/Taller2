using UnityEngine;

public class ArrowPulse : MonoBehaviour
{
    public float speed = 3f;        // Velocidad del pulso
    public float scaleAmount = 0.2f; // Qué tanto crece

    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = startScale * scale;
    }
}
