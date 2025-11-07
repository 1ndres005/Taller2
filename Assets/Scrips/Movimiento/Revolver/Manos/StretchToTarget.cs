using UnityEngine;

public class StretchToTarget : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target;          // el punto al que se estira
    public Transform basePoint;       // base fija (opcional, puede ser el mismo objeto)
    
    [Header("Configuración")]
    public bool lookAtTarget = true;  // hace que la cápsula apunte al target
    public float smoothSpeed = 10f;   // suavizado del estiramiento

    private Vector3 originalScale;

    void Start()
    {
        if (basePoint == null)
            basePoint = transform; // por defecto, usa el mismo objeto
        
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (target == null) return;

        // Calcula distancia entre base y target
        float distance = Vector3.Distance(basePoint.position, target.position);

        // Escala solo en el eje Z (suponiendo que la cápsula está orientada hacia adelante)
        Vector3 newScale = new Vector3(originalScale.x, originalScale.y, distance);

        // Interpolación suave
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * smoothSpeed);

        if (lookAtTarget)
        {
            // Rota el objeto para mirar hacia el target
            transform.LookAt(target);
        }
    }
}
