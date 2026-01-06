using UnityEngine;

public class CameraTargetSmooth : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Offset (altura del punto que mira)")]
    public Vector3 offset = new Vector3(0f, 1.6f, 0f);

    [Header("Smoothing")]
    [Range(0.01f, 0.3f)]
    public float smoothTime = 0.08f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (!player) return;

        Vector3 desired = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
