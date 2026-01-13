using UnityEngine;

public class ThirdPersonCameraRig : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Transform camTransform;

    [Header("Distance / Zoom")]
    public float distance = 5f;
    public float minDistance = 1.5f;
    public float maxDistance = 8f;
    public float zoomSpeed = 0.02f; // pinch sensitivity

    [Header("Rotation (Touch Drag)")]
    public float touchSensitivity = 0.15f;
    public float minY = -30f;
    public float maxY = 60f;

    [Header("Collision")]
    public float collisionRadius = 0.3f;
    public float collisionPadding = 0.2f;
    public LayerMask collisionLayers;

    float yaw;
    float pitch = 15f;

    // pinch
    float prevPinchDist;

    void Start()
    {
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void LateUpdate()
    {
        if (!target || !camTransform) return;

        HandleTouchRotationAndZoom();

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 focusPoint = target.position;
        Vector3 desiredPos = focusPoint - rotation * Vector3.forward * distance;

        RaycastHit hit;
        Vector3 dir = (desiredPos - focusPoint).normalized;

        if (Physics.SphereCast(focusPoint, collisionRadius, dir, out hit, distance, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float hitDist = Mathf.Max(hit.distance - collisionPadding, minDistance);
            desiredPos = focusPoint + dir * hitDist;
        }

        camTransform.position = desiredPos;
        camTransform.LookAt(focusPoint);
    }

    void HandleTouchRotationAndZoom()
    {
        // ✅ Mobile: 1 dedo = rotar
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector2 d = t.deltaPosition;
                yaw += d.x * touchSensitivity;
                pitch -= d.y * touchSensitivity;
                pitch = Mathf.Clamp(pitch, minY, maxY);
            }
        }

        // ✅ Mobile: 2 dedos = zoom (pinch)
        if (Input.touchCount == 2)
        {
            Touch a = Input.GetTouch(0);
            Touch b = Input.GetTouch(1);

            float currDist = Vector2.Distance(a.position, b.position);

            if (a.phase == TouchPhase.Began || b.phase == TouchPhase.Began)
                prevPinchDist = currDist;

            float delta = currDist - prevPinchDist;
            prevPinchDist = currDist;

            distance -= delta * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // ✅ PC test (opcional): mouse
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButton(0))
            {
                yaw += Input.GetAxis("Mouse X") * 3f;
                pitch -= Input.GetAxis("Mouse Y") * 3f;
                pitch = Mathf.Clamp(pitch, minY, maxY);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                distance -= scroll * 2f;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }
        }
    }
}
