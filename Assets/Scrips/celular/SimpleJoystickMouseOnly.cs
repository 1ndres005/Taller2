using UnityEngine;

public class SimpleJoystickMouseOnly : MonoBehaviour
{
    [Header("References")]
    public RectTransform joystickBG;
    public RectTransform handle;

    [Header("Settings")]
    public float radius = 80f;

    private Canvas canvas;
    private Camera uiCamera;

    private bool isActive = false; // ⬅️ SOLO si se toca el joystick
    private Vector2 input;

    public Vector2 InputVector => input;

    void Awake()
    {
        if (joystickBG == null)
            joystickBG = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();

        uiCamera = null;
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCamera = canvas.worldCamera;
    }

    void Update()
    {
        // CLICK / TOUCH INICIO
        if (Input.GetMouseButtonDown(0))
        {
            // ¿El click fue sobre el joystick?
            if (RectTransformUtility.RectangleContainsScreenPoint(
                joystickBG,
                Input.mousePosition,
                uiCamera))
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }

        // MOVER SOLO SI ESTÁ ACTIVO
        if (isActive && Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBG,
                Input.mousePosition,
                uiCamera,
                out Vector2 localPos
            );

            Vector2 clamped = Vector2.ClampMagnitude(localPos, radius);

            handle.anchoredPosition = clamped;
            input = clamped / radius;
        }

        // SOLTAR
        if (Input.GetMouseButtonUp(0))
        {
            isActive = false;
            handle.anchoredPosition = Vector2.zero;
            input = Vector2.zero;
        }
    }
}
