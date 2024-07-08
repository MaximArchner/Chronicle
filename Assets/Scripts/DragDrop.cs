using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;



    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //Böylelikle ray cast öðeyi görmezden gelecek
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Öðe faremizle ayný hýzda hareket edecek
        rectTransform.anchoredPosition += eventData.delta;

    }



    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
        itemBeingDragged = null;

        //if(tempItemReference.transform.parent == tempItemReference.transform.root)
        //{
        //  tempItemReference.SetActive(false);

        //  AlertDialogManager dialogManager = FindObjectOfType<AlaertDialogManager>();

        // dialogManager.ShowDialog("Do you want to drop this item?, (response) =>
        //  {
        //      if (response)
        //      {
        //          DropItemIntoTheWorld(tempItemReference);
        //      }
        //      else
        //      {
        //          CancelDragging();
        //      }
        //  }
        //}

        //if(tempItemReference.transform.parent == startParent)
        //{
        //  CancelDragging();
        //}

        //if (tempItemReference.transform.parent != tempItemReference.transform.root && tempItemReference.transform.parent != startParent)
        //{
        //  if (tempItemReference.transform.parent.childCount > 98)
        //  {
        //      CancelDragging();
        //      Debug.Log("Was not accepted into this slot.");
        //  }
        //  else
        //  {
        //      if (Input.GetKey(KeyCode.Z))
        //      {
        //          DivideStack(tempItemReference);
        //      }
        //      Debug.Log("Should be moved to another slot.");
        //  }
        //}

        if (transform.parent == startParent || transform.parent == transform.root) // to be discarded in 40
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
            //CancelDragging(tempItemReference);

        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    //private void DivideStack(GameObject tempItemReference)
    //{
    //    InventoryItem item = tempItemReference.GetComponent<InventoryItem>();

    //    if(item.amountInInventory > 1)
    //    {
    //        item.amountInInventory -= 1;
    //        InventorySystem.Instance.AddToInventory(item.thisName, false);
    //    }
    //}

    //void CancelDragging(GameObject tempItemReference)
    //{
    //    transform.position = startPosition;
    //    transform.SetParent(startParent);
    //}
}
