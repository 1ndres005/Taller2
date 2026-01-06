using UnityEngine;

public class ThirdPersonCameraRig : MonoBehaviour
{
    [Header("References")]
    public Transform target;          // Arrastra CameraTargetSmooth aquí
    public Transform camTransform;    // Arrastra Main Camera aquí

    [Header("Distance / Zoom")]
    public float distance = 5f;
    public float minDistance = 1.5f;
    public float maxDistance = 8f;
    public float zoomSpeed = 2f;

    [Header("Rotation (solo mouse)")]
    public float mouseSensitivity = 3f;
    public float minY = -30f;
    public float maxY = 60f;
    public bool rotateOnlyWhileRightMouseHeld = false;

    [Header("Collision")]
    public float collisionRadius = 0.3f;
    public float collisionPadding = 0.2f;
    public LayerMask collisionLayers;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target || !camTransform) return;

        bool canRotate = !rotateOnlyWhileRightMouseHeld || Input.GetMouseButton(1);

        if (canRotate)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minY, maxY);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 focusPoint = target.position;
        Vector3 desiredPos = focusPoint - rotation * Vector3.forward * distance;

        RaycastHit hit;
        Vector3 dir = (desiredPos - focusPoint).normalized;

        if (Physics.SphereCast(
            focusPoint,
            collisionRadius,
            dir,
            out hit,
            distance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            float hitDist = Mathf.Max(hit.distance - collisionPadding, minDistance);
            desiredPos = focusPoint + dir * hitDist;
        }

        camTransform.position = desiredPos;
        camTransform.LookAt(focusPoint);
    }
}
