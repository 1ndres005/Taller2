using UnityEngine;
using UnityEngine.UI;

public class ScrollUITexture : MonoBehaviour
{
    public float speedX = 0.2f;
    public float speedY = 0.0f;

    private RawImage rawImage;
    private Vector2 offset;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        offset.x += speedX * Time.deltaTime;
        offset.y += speedY * Time.deltaTime;

        rawImage.uvRect = new Rect(offset, rawImage.uvRect.size);
    }
}
