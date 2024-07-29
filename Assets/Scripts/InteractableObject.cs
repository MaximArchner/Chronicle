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

    public TextMeshPro proximityText;


    private static List<InteractableObject> objectsInRange = new List<InteractableObject>();
    public InteractableObject ClosestObject { get; set; }

    private static List<InteractableObject> npcInRange = new List<InteractableObject>();
    public InteractableObject ClosestNPC { get; set; }

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
        
        if (!CompareTag("NPC"))
        {
            npcName = null;
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
                Debug.Log("Item added into the inventory.");
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

        // Huntable/Choppable Interaction
        if (entityInfoUI != null && (CompareTag("Choppable") || CompareTag("Killable")))
        {
            ClosestObject = null;
            float closestDistance = float.MaxValue;

            foreach (var obj in objectsInRange)
            {
                if (obj == null) continue;

                float distance = Vector3.Distance(player.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    ClosestObject = obj;
                }
            }

            if (ClosestObject != null && entityInfoUI != null)
            {
                entityInfoUI.SetActive(true);
                entityInfoUI.transform.Find("EntityName").GetComponent<TextMeshProUGUI>().text = ClosestObject.ItemName;
                entityInfoUI.transform.Find("EntityDescription").GetComponent<TextMeshProUGUI>().text = ClosestObject.entityDescription;
                entityInfoUI.transform.Find("EntityImage").GetComponent<Image>().sprite = ClosestObject.entityImage;
            }
        }

        // NPC Interaction
        if (CompareTag("NPC"))
        {
            ClosestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (var npc in npcInRange)
            {
                if (npc == null) continue;

                float distance = Vector3.Distance(player.position, npc.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    ClosestNPC = npc;
                }
            }
        }

        if (ClosestNPC != null && ClosestNPC.npcName == "Samantha" && Input.GetKeyDown(KeyCode.F))
        { 
            proximityText.text = npcName;
            ItemName = npcName;
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
                else if (ClosestNPC != null && CompareTag("NPC"))
                {
                    ClosestNPC.proximityText.text = ItemName;
                }
                else if (CompareTag("Killable") || !npc.nameLearned)
                {
                    proximityText.text = ItemName;
                }
            }

            if (playerInRange && entityInfoUI != null && (CompareTag("Choppable") || CompareTag("Killable")))
            {
                objectsInRange.Add(this);
                entityInfoUI.SetActive(true);
                entityInfoUI.transform.Find("EntityName").transform.GetComponent<TextMeshProUGUI>().text = ItemName;
                entityInfoUI.transform.Find("EntityDescription").transform.GetComponent<TextMeshProUGUI>().text = entityDescription;
                entityInfoUI.transform.Find("EntityImage").GetComponent<Image>().sprite = entityImage;
            }

            if (CompareTag("NPC"))
            {
                npcInRange.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            objectsInRange.Remove(this);

            if (proximityText != null && (CompareTag("Collectible") || CompareTag("Killable") || CompareTag("NPC")))
            {
                proximityText.gameObject.SetActive(false);
            }

            if ((CompareTag("Choppable") || CompareTag("Killable")) && entityInfoUI != null)
            {
                entityInfoUI.SetActive(false);
            }

            if (CompareTag("NPC"))
            {
                npcInRange.Remove(this);
            }
        }
    }
}
