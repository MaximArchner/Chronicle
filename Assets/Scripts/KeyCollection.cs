using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollection : MonoBehaviour
{
    public bool playerInRange = false;
    public string ItemName;

    private void Update()
    {
        ItemName = transform.gameObject.name;
        
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                InventorySystem.Instance.AddToInventory(ItemName, false);
                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
