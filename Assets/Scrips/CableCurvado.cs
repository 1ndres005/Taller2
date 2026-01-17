using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableCurvado : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;

    [Header("Forma")]
    [Range(2, 80)] public int segmentos = 25;
    [Range(0f, 5f)] public float curvatura = 0.8f; // cuánto cuelga

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = segmentos;
    }

    void LateUpdate()
    {
        if (!puntoA || !puntoB) return;

        Vector3 a = puntoA.position;
        Vector3 b = puntoB.position;

        for (int i = 0; i < segmentos; i++)
        {
            float t = i / (float)(segmentos - 1);

            // Línea base entre A y B
            Vector3 p = Vector3.Lerp(a, b, t);

            // Curva en el centro (parábola): 0 en extremos, 1 en el centro
            float centro = 4f * t * (1f - t);

            // Baja en Y según curvatura
            p.y -= centro * curvatura;

            lr.SetPosition(i, p);
        }
    }
}
