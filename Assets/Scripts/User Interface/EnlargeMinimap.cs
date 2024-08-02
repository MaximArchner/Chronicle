using UnityEngine;

public class EnlargeMinimap : MonoBehaviour
{
    public Camera minimapCamera;
    public RectTransform minimapRectTransform;
    public RectTransform questTrackerScrollView; // Reference to the ScrollView's RectTransform
    private float initialWidth;
    private float initialHeight;
    private Vector2 initialAnchorMin;
    private Vector2 initialAnchorMax;
    private Vector2 initialAnchoredPosition;
    private Vector2 initialQuestTrackerPosition;
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

        if (questTrackerScrollView != null)
        {
            initialQuestTrackerPosition = questTrackerScrollView.anchoredPosition;
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

                // Move the quest tracker scroll view down
                if (questTrackerScrollView != null)
                {
                    questTrackerScrollView.anchoredPosition = initialQuestTrackerPosition + new Vector2(0, -100);
                }
            }

            // Ensure anchors are preserved
            minimapRectTransform.anchorMin = initialAnchorMin;
            minimapRectTransform.anchorMax = initialAnchorMax;
        }

        if (!Input.GetKey(KeyCode.Tab))
        {
            if (isMapEnlarged)
            {
                // Restore the minimap to its original size
                isMapEnlarged = false;
                minimapCamera.orthographicSize = 45;
                minimapRectTransform.sizeDelta = new Vector2(initialWidth, initialHeight);
                minimapRectTransform.anchoredPosition = initialAnchoredPosition;

                // Restore the quest tracker scroll view position
                if (questTrackerScrollView != null)
                {
                    questTrackerScrollView.anchoredPosition = initialQuestTrackerPosition;
                }
            }
        }
    }
}

