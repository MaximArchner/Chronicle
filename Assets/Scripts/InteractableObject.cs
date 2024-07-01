using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    
    public string ItemName;

    public TextMeshPro proximityText; // yakina gelince bu objeyi tweaklemeli

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerInRange && CompareTag("Collectible")) // Objenin collider'ina dokunuyorken ve objenin tag'i Collectible ise
        {
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Debug.Log("Item added into the inventory.");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Objenin collider'ina dokundugumuzda text olusturmali ve Range'inde oldugumuzu bildirmeli
        {
            playerInRange = true;
            proximityText.gameObject.SetActive(true);
            // proximityText'in transform degerlerini ve oyuncuya bakmasini saglamaliyiz
            proximityText.text = gameObject.name + " [E]";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            proximityText.gameObject.SetActive(false);
        }
    }
}
