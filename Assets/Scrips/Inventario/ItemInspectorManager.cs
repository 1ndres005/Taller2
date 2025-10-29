using UnityEngine;

public class ItemInspectorManager : MonoBehaviour
{
    public static ItemInspectorManager Instance;
    private void Awake() => Instance = this;

    public Transform inspectPosition;
    public GameObject currentInspected;

    private bool isInspecting = false;

    void Update()
    {
        if (isInspecting && currentInspected != null)
        {
            Vector2 mouseDelta = GameInput.Instance.GetMouseDelta();
            float rotX = mouseDelta.x * 0.2f;
            float rotY = mouseDelta.y * 0.2f;

            currentInspected.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentInspected.transform.Rotate(Vector3.right, rotY, Space.World);

            // 🔹 Si presiona Escape → cerrar inspección
            if (GameInput.Instance.CancelPressed())
            {
                HideItem();
            }
        }
    }

    public void ShowItem(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("No hay prefab 3D asignado para inspeccionar.");
            return;
        }

        if (currentInspected != null)
            Destroy(currentInspected);

        currentInspected = Instantiate(prefab, inspectPosition.position, Quaternion.identity);
        currentInspected.transform.LookAt(Camera.main.transform);
        isInspecting = true;
    }

    public void HideItem()
    {
        if (currentInspected != null)
            Destroy(currentInspected);

        isInspecting = false;
    }
}
