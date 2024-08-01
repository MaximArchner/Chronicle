using UnityEngine;

public class EnlargeMinimap : MonoBehaviour
{
    public Camera minimapCamera;
    public RectTransform minimapRectTransform;
    private float initialWidth;
    private float initialHeight;
    private Vector2 initialAnchorMin;
    private Vector2 initialAnchorMax;
    private Vector2 initialAnchoredPosition;
    private bool isMapEnlarged;

    void Start()
    {
        minimapRectTransform = GetComponent<RectTransform>();

        if (minimapRectTransform != null)
        {
            initialWidth = minimapRectTransform.rect.width;
            initialHeight = minimapRectTransform.rect.height;
            initialAnchorMin = minimapRectTransform.anchorMin;
            initialAnchorMax = minimapRectTransform.anchorMax;
            initialAnchoredPosition = minimapRectTransform.anchoredPosition;
        }

        isMapEnlarged = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!isMapEnlarged)
            {
                // Enlarge the minimap
                isMapEnlarged = true;
                minimapCamera.orthographicSize = 90;
                minimapRectTransform.sizeDelta = new Vector2(300, 300);
                minimapRectTransform.anchoredPosition = new Vector3(-200, -200, 0);
            }

            // Ensure anchors are preserved
            minimapRectTransform.anchorMin = initialAnchorMin;
            minimapRectTransform.anchorMax = initialAnchorMax;
        }

        if (!Input.GetKey(KeyCode.Tab))
        {
            // Restore the minimap to its original size
            isMapEnlarged = false;
            minimapCamera.orthographicSize = 45;
            minimapRectTransform.sizeDelta = new Vector2(initialWidth, initialHeight);
            minimapRectTransform.anchoredPosition = initialAnchoredPosition;
        }
    }
}
