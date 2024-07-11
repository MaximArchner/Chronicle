using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler
{

    //public GameObject Item
    //{
    //    get
    //    {
    //        if (transform.childCount > 0)
    //        {
    //            return transform.GetChild(0).gameObject;
    //        }

    //        return null;
    //    }
    //}


    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= 1) //mevcut slot bos ise
        {

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            /*if (transform.CompareTag("Quickslot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();
            }

            if (transform.CompareTag("Quickslot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                InventorySystem.Instance.ReCalculateList();
            }*/
        }
        else //mevcut slot bos degil ise
        {
            InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();

            // iki item'in da ayni tipten olup olmadigini anlamak icin
            if (draggedItem.thisName == GetStoredItem().thisName && IsLimitExceeded(draggedItem) == false)
            {
                // DraggedItem ile StoredItem'ý mergeleme
                GetStoredItem().amountInInventory += draggedItem.amountInInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }

    }

    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    bool IsLimitExceeded(InventoryItem draggedItem)
    {
        if((draggedItem.amountInInventory + GetStoredItem().amountInInventory) > InventorySystem.Instance.stackLimit) 
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
