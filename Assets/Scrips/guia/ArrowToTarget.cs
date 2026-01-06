using UnityEngine;

public class ArrowToTarget: MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    void Update()
    {
        if (!target) return;

        Vector3 direction = target.position - transform.position;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
