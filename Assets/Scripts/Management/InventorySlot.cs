using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI ItemCount;
    public InventoryItem itemInSlot;

    private void Update()
    {
        InventoryItem item = CheckInventoryItem();

        if (item != null)
        {
            itemInSlot = item;
        }

        else
        {
            itemInSlot = null;
        }

        if (itemInSlot != null && itemInSlot.isStackable)
        {
            ItemCount.gameObject.SetActive(true);
            ItemCount.text = $"{itemInSlot.amountInInventory}";
            ItemCount.transform.SetAsLastSibling();
        }
        else
        {
            ItemCount.gameObject.SetActive(false);
            ItemCount.transform.SetAsLastSibling();
        }
    }

    private InventoryItem CheckInventoryItem()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }

        return null;
    }

    public void UpdateItemInSlot()
    {
        itemInSlot = CheckInventoryItem();
    }
}
