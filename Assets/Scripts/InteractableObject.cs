using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    
    public string ItemName;

    public TextMeshPro proximityText; // Reference to the TextMeshPro element
    public Vector3 textOffset = new Vector3(0, 2, 0);

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.E) && playerInRange)
        {
            Debug.Log("Item added into the inventory.");
            Destroy(gameObject);
        }
        else if (Input.GetKey(KeyCode.E) && selectionManager.Instance.onTarget && playerInRange)
        {
            Debug.Log("Item added into the inventory.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            proximityText.gameObject.SetActive(true);
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
