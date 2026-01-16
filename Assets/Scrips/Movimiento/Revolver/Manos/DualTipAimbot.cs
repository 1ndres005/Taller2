using UnityEngine;

public class DualTipAimbot : MonoBehaviour
{
    // =========================
    // ENUM PARA FREEZE ROTATION
    // =========================
    public enum FreezeRotationAxis
    {
        None,
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }

    [Header("Referencias de puntas")]
    public Transform leftTip;
    public Transform rightTip;

    [Header("Target")]
    public Transform target;

    [Header("Controles")]
    public bool leftUsesLeftClick = true;
    public bool leftUsesRightClick = false;
    public bool rightUsesRightClick = true;
    public bool rightUsesLeftClick = false;

    [Header("Movimiento")]
    public float moveSpeed = 12f;
    public float stopDistanceFromTarget = 0.05f;

    [Header("Distance Offset")]
    [Range(0f, 1f)]
    public float distanceOffset = 1f; // 0 = no se acerca | 1 = comportamiento original

    public bool rotateToLookAtTarget = true;

    [Header("Freeze Rotation")]
    public FreezeRotationAxis freezeRotation = FreezeRotationAxis.None;

    [Header("Rotation Offset")]
    [Range(0f, 1f)]
    public float rotationOffset = 1f; // 0 = no rota | 1 = comportamiento original

    // originales
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

        if (rightTip != null)
        {
            rightOriginalLocalPos = rightTip.localPosition;
            rightOriginalLocalRot = rightTip.localRotation;
        }
    }

    void Update()
    {
        bool leftPressed = false;
        bool rightPressed = false;

        if (leftUsesLeftClick && Input.GetMouseButton(0)) leftPressed = true;
        if (leftUsesRightClick && Input.GetMouseButton(1)) leftPressed = true;

        if (rightUsesRightClick && Input.GetMouseButton(1)) rightPressed = true;
        if (rightUsesLeftClick && Input.GetMouseButton(0)) rightPressed = true;

        if (target == null)
        {
            if (leftTip != null) ReturnTipToOriginal(leftTip, leftOriginalLocalPos, leftOriginalLocalRot);
            if (rightTip != null) ReturnTipToOriginal(rightTip, rightOriginalLocalPos, rightOriginalLocalRot);
            return;
        }

        if (leftTip != null)
        {
            if (leftPressed)
                MoveTipTowardsTarget(leftTip);
            else
                ReturnTipToOriginal(leftTip, leftOriginalLocalPos, leftOriginalLocalRot);
        }

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
        Vector3 dir = (tip.position - target.position).normalized;
        if (dir == Vector3.zero) dir = tip.forward;

        // 🔹 DISTANCE OFFSET 0–1 (NO rompe comportamiento)
        float effectiveDistance = stopDistanceFromTarget * distanceOffset;
        Vector3 desiredWorldPos = target.position + dir * effectiveDistance;

        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
        tip.position = Vector3.Lerp(tip.position, desiredWorldPos, t);

        if (rotateToLookAtTarget)
        {
            Quaternion desiredRot = Quaternion.LookRotation(target.position - tip.position, Vector3.up);

            Quaternion offsetRot = Quaternion.Slerp(tip.rotation, desiredRot, rotationOffset);
            offsetRot = ApplyRotationFreeze(tip.rotation, offsetRot);

            tip.rotation = Quaternion.Slerp(tip.rotation, offsetRot, t);
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

    // =========================
    // FUNCIÓN DE FREEZE ROTATION
    // =========================
    Quaternion ApplyRotationFreeze(Quaternion current, Quaternion desired)
    {
        Vector3 currentEuler = current.eulerAngles;
        Vector3 desiredEuler = desired.eulerAngles;

        switch (freezeRotation)
        {
            case FreezeRotationAxis.X:
                desiredEuler.x = currentEuler.x;
                break;
            case FreezeRotationAxis.Y:
                desiredEuler.y = currentEuler.y;
                break;
            case FreezeRotationAxis.Z:
                desiredEuler.z = currentEuler.z;
                break;
            case FreezeRotationAxis.XY:
                desiredEuler.x = currentEuler.x;
                desiredEuler.y = currentEuler.y;
                break;
            case FreezeRotationAxis.XZ:
                desiredEuler.x = currentEuler.x;
                desiredEuler.z = currentEuler.z;
                break;
            case FreezeRotationAxis.YZ:
                desiredEuler.y = currentEuler.y;
                desiredEuler.z = currentEuler.z;
                break;
            case FreezeRotationAxis.XYZ:
                desiredEuler = currentEuler;
                break;
        }

        return Quaternion.Euler(desiredEuler);
    }
}
