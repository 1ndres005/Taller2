using UnityEngine;

public class ArrowToTargets : MonoBehaviour
{
    [Header("Targets (en orden)")]
    public Transform[] targets;   // Arrastra aquí todos los objetivos en orden

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    private int currentIndex = 0;

    void Update()
    {
        Transform currentTarget = GetCurrentTarget();

        // 🔴 Si ya no hay más objetivos → apagar flecha
        if (currentTarget == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 direction = currentTarget.position - transform.position;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    Transform GetCurrentTarget()
    {
        // Avanza automáticamente hasta encontrar un target válido
        while (currentIndex < targets.Length)
        {
            if (targets[currentIndex] != null &&
                targets[currentIndex].gameObject.activeInHierarchy)
            {
                return targets[currentIndex];
            }

            // Si este target ya no existe o está apagado → siguiente
            currentIndex++;
        }

        // No quedan más objetivos
        return null;
    }
}
