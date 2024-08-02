using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();
        if (transform.childCount == 1)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;

            if (!transform.CompareTag("QuickSlot"))
            {
                draggedItem.isInsideQuickSlot = false;
                CraftingSystem.Instance.RefreshNeededItems();
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else if (transform.childCount == 2)
        {
            if (draggedItem.thisName == GetStoredItem().thisName && !IsLimitExceeded(draggedItem) && draggedItem.isStackable)
            {
                GetStoredItem().amountInInventory += draggedItem.amountInInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
        }

        if (transform.CompareTag("QuickSlot"))
        {
            if (draggedItem.isEquippable)
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
                DragDrop.itemBeingDragged.transform.localPosition = Vector2.zero;
                draggedItem.isInsideQuickSlot = true;
            }

            CraftingSystem.Instance.RefreshNeededItems();
            InventorySystem.Instance.ReCalculateList();
        }
    }

    private InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    private bool IsLimitExceeded(InventoryItem draggedItem)
    {
        return (draggedItem.amountInInventory + GetStoredItem().amountInInventory) > InventorySystem.Instance.stackLimit;
    }
}
