using UnityEngine;

public class DualTipAimbot : MonoBehaviour
{
    [Header("Referencias de puntas")]
    public Transform leftTip;   // la punta/objeto visible de la mano izquierda
    public Transform rightTip;  // la punta/objeto visible de la mano derecha

    [Header("Target")]
    public Transform target;    // la bolita del medio

    [Header("Controles")]
    public bool leftUsesLeftClick = true;   // leftTip con click izquierdo (mouse 0)
    public bool leftUsesRightClick = false; // si quieres cambiar controles (no necesario)
    public bool rightUsesRightClick = true; // rightTip con click derecho (mouse 1)
    public bool rightUsesLeftClick = false;

    [Header("Movimiento")]
    public float moveSpeed = 12f;             // rapidez del lerp
    public float stopDistanceFromTarget = 0.05f; // distancia desde el target donde quedará la punta
    public bool rotateToLookAtTarget = true;  // si la punta rota mirando al target

    // almacenan las posiciones/rotaciones locales originales para regresar al soltar
    private Vector3 leftOriginalLocalPos;
    private Quaternion leftOriginalLocalRot;
    private Vector3 rightOriginalLocalPos;
    private Quaternion rightOriginalLocalRot;

    void Start()
    {
        if (leftTip != null)
        {
            leftOriginalLocalPos = leftTip.localPosition;
            leftOriginalLocalRot = leftTip.localRotation;
        }
        else
            Debug.LogWarning("[DualTipAimbot] leftTip no asignado.");

        if (rightTip != null)
        {
            rightOriginalLocalPos = rightTip.localPosition;
            rightOriginalLocalRot = rightTip.localRotation;
        }
        else
            Debug.LogWarning("[DualTipAimbot] rightTip no asignado.");

        if (target == null)
            Debug.LogWarning("[DualTipAimbot] target no asignado.");
    }

    void Update()
    {
        // determinar inputs
        bool leftPressed = false;
        bool rightPressed = false;

        // por defecto: leftTip con mouse0, rightTip con mouse1. Las flags permiten otra configuración si quieres.
        if (leftUsesLeftClick && Input.GetMouseButton(0)) leftPressed = true;
        if (leftUsesRightClick && Input.GetMouseButton(1)) leftPressed = true;

        if (rightUsesRightClick && Input.GetMouseButton(1)) rightPressed = true;
        if (rightUsesLeftClick && Input.GetMouseButton(0)) rightPressed = true;

        // Si target nulo, no hacer aimbot
        if (target == null)
        {
            // regresar ambos si existan
            if (leftTip != null) ReturnTipToOriginal(leftTip, leftOriginalLocalPos, leftOriginalLocalRot);
            if (rightTip != null) ReturnTipToOriginal(rightTip, rightOriginalLocalPos, rightOriginalLocalRot);
            return;
        }

        // Left tip
        if (leftTip != null)
        {
            if (leftPressed)
                MoveTipTowardsTarget(leftTip);
            else
                ReturnTipToOriginal(leftTip, leftOriginalLocalPos, leftOriginalLocalRot);
        }

        // Right tip
        if (rightTip != null)
        {
            if (rightPressed)
                MoveTipTowardsTarget(rightTip);
            else
                ReturnTipToOriginal(rightTip, rightOriginalLocalPos, rightOriginalLocalRot);
        }
    }

    void MoveTipTowardsTarget(Transform tip)
    {
        // Calculamos dirección desde target hacia la punta actual (para evitar superposición).
        Vector3 dir = (tip.position - target.position).normalized;
        if (dir == Vector3.zero) dir = tip.forward; // fallback

        Vector3 desiredWorldPos = target.position + dir * stopDistanceFromTarget;

        // Movimiento suave usando un exponencial (se siente más natural que Lerp directo)
        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
        tip.position = Vector3.Lerp(tip.position, desiredWorldPos, t);

        if (rotateToLookAtTarget)
        {
            Quaternion desiredRot = Quaternion.LookRotation(target.position - tip.position, Vector3.up);
            tip.rotation = Quaternion.Slerp(tip.rotation, desiredRot, t);
        }
    }

    void ReturnTipToOriginal(Transform tip, Vector3 originalLocalPos, Quaternion originalLocalRot)
    {
        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);

        if (tip.parent != null)
        {
            Vector3 targetWorldPos = tip.parent.TransformPoint(originalLocalPos);
            tip.position = Vector3.Lerp(tip.position, targetWorldPos, t);

            Quaternion targetWorldRot = tip.parent.rotation * originalLocalRot;
            tip.rotation = Quaternion.Slerp(tip.rotation, targetWorldRot, t);
        }
        else
        {
            tip.localPosition = Vector3.Lerp(tip.localPosition, originalLocalPos, t);
            tip.localRotation = Quaternion.Slerp(tip.localRotation, originalLocalRot, t);
        }
    }
}
