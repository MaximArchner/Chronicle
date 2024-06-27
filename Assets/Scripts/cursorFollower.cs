using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cursorFollower : MonoBehaviour
{
    public RectTransform canvasRectTransform; // Reference to the canvas RectTransform
    private RectTransform imageRectTransform; // Reference to the image RectTransform
    public Vector2 cursorOffset = Vector2.right + Vector2.down;

    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out localPoint);
        imageRectTransform.localPosition = localPoint + cursorOffset;
    }
}
