using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    private Transform player;
    public Vector3 textOffset = new Vector3(0, 0, 0);
    public string ItemName;
    public string entityDescription;
    public Sprite entityImage;
    public GameObject entityInfoUI;
    public string npcName;

    public bool isBeingDestroyed = false;

    public TextMeshPro proximityText;
    public string GetItemName()
    {
        return ItemName;
    }

    private void Start()
    {
        player = PlayerState.Instance.playerBody.transform.Find("Main Camera").transform;

        if (CompareTag("Collectible") || CompareTag("NPC"))
        {
            entityInfoUI = null;
        }
        else if (CompareTag("Choppable"))
        {
            proximityText = null;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && CompareTag("Collectible"))
        {

            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                InventorySystem.Instance.AddToInventory(ItemName, true);
                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }

        if (playerInRange && proximityText != null && (CompareTag("Collectible") || CompareTag("Killable") || CompareTag("NPC")))
        {
            proximityText.transform.position = proximityText.transform.parent.position + textOffset;
            proximityText.transform.LookAt(player.transform);
            proximityText.transform.Rotate(0, 180, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            NPC npc = GetComponent<NPC>();

            if (proximityText != null)
            {
                proximityText.gameObject.SetActive(true);
                proximityText.transform.position = proximityText.transform.parent.position + textOffset;
                proximityText.transform.LookAt(player.transform);
                proximityText.transform.Rotate(0, 180, 0);

                if (CompareTag("Collectible"))
                {
                    proximityText.text = ItemName + " [E]";
                }
                else if (CompareTag("Killable") || !npc.nameLearned)
                {
                    proximityText.text = ItemName;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (proximityText != null && (CompareTag("Collectible") || CompareTag("Killable") || CompareTag("NPC")))
            {
                proximityText.gameObject.SetActive(false);
            }

            if ((CompareTag("Choppable") || CompareTag("Killable")) && entityInfoUI != null)
            {
                entityInfoUI.SetActive(false);
            }
        }
    }
}
