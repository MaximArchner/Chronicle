using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollection : MonoBehaviour
{
    public bool playerInRange = false;
    public string ItemName;

    private void Start()
    {
        if (transform.gameObject.name.Contains("_Model"))
        {
            ItemName = transform.gameObject.name.Replace("_Model", "");
        }
        else
        {
            ItemName=transform.gameObject.name;
        }
    }
    private void Update()
    {
        
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                if (transform.gameObject.name.Contains("Key"))
                {
                    InventorySystem.Instance.AddToInventory(ItemName, false);
                }
                else
                {
                    InventorySystem.Instance.AddToInventory(ItemName, true);
                }
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
