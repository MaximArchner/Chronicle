using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler
{

    //public GameObject Item Bu belki daha sonra kullanılır
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

        InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();
        if (transform.childCount == 1) //mevcut slot bos ise
        {

            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                draggedItem.isInsideQuickSlot = false;
                CraftingSystem.Instance.RefreshNeededItems();
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else if (transform.childCount == 2)//mevcut slot bos degil ise
        {
            // iki item'in da ayni tipten olup olmadigini anlamak icin
            if (draggedItem.thisName == GetStoredItem().thisName && IsLimitExceeded(draggedItem) == false && draggedItem.isStackable == true)
            {
                // DraggedItem ile StoredItem'� mergeleme
                GetStoredItem().amountInInventory += draggedItem.amountInInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
        }

        if (transform.CompareTag("QuickSlot"))
        {
            if (draggedItem.isEquippable == true)
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
                DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
                draggedItem.isInsideQuickSlot = true;
            }

            CraftingSystem.Instance.RefreshNeededItems();
            InventorySystem.Instance.ReCalculateList();
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
