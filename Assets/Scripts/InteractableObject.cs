using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public Transform player;
    public Vector3 textOffset = new Vector3(0, 0, 0);
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

        if (playerInRange)
        {
            proximityText.transform.position = proximityText.transform.parent.position + textOffset;
            proximityText.transform.LookAt(player.transform);
            proximityText.transform.Rotate(0, 180, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Objenin collider'ina dokundugumuzda text olusturmali ve Range'inde oldugumuzu bildirmeli
        {
            playerInRange = true;
            proximityText.gameObject.SetActive(true);
            proximityText.transform.position = proximityText.transform.parent.position + textOffset;
            proximityText.transform.LookAt(player.transform);
            proximityText.transform.Rotate(0, 180, 0);
            if (CompareTag("Collectible"))
            {
                proximityText.text = ItemName + " [E]";
            }
            else if (!CompareTag("Collectible"))
            {
                proximityText.text = ItemName;
            }
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
