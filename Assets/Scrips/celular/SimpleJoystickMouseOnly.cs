using UnityEngine;

public class SimpleJoystickMouseOnly : MonoBehaviour
{
    [Header("Handle")]
    public RectTransform handle;

    [Header("Settings")]
    public float radius = 80f;

    private Vector2 input;

    public Vector2 InputVector => input;

    Vector2 startMousePos;

    void Update()
    {
        // Click izquierdo = mover joystick
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - startMousePos;
            Vector2 clamped = Vector2.ClampMagnitude(delta, radius);

            handle.anchoredPosition = clamped;
            input = clamped / radius;
        }

        if (Input.GetMouseButtonUp(0))
        {
            handle.anchoredPosition = Vector2.zero;
            input = Vector2.zero;
        }
    }
}
