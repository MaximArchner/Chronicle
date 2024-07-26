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

    public TextMeshPro proximityText; // yakina gelince bu objeyi tweaklemeli

    private static List<InteractableObject> objectsInRange = new List<InteractableObject>();

    public string GetItemName()
    {
        return ItemName;
    }

    private void Start()
    {
        player = PlayerState.Instance.playerBody.transform.Find("Main Camera").transform;

        if (CompareTag("Collectible"))
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
        if(Input.GetKeyDown(KeyCode.E) && playerInRange && CompareTag("Collectible")) // Objenin collider'ina dokunuyorken ve objenin tag'i Collectible ise
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

        if (playerInRange && proximityText != null && CompareTag("Collectible"))
        {
            proximityText.transform.position = proximityText.transform.parent.position + textOffset;
            proximityText.transform.LookAt(player.transform);
            proximityText.transform.Rotate(0, 180, 0);
        }

        if (entityInfoUI != null && (CompareTag("Choppable") || CompareTag("Killable")))
        {
            InteractableObject closestObject = null;
            float closestDistance = float.MaxValue;

            foreach (var obj in objectsInRange)
            {
                float distance = Vector3.Distance(player.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }

            if (closestObject != null && entityInfoUI != null)
            {
                entityInfoUI.SetActive(true);
                entityInfoUI.transform.Find("EntityName").GetComponent<TextMeshProUGUI>().text = closestObject.ItemName;
                entityInfoUI.transform.Find("EntityDescription").GetComponent<TextMeshProUGUI>().text = closestObject.entityDescription;
                entityInfoUI.transform.Find("EntityImage").GetComponent<Image>().sprite = closestObject.entityImage;
            }

            if (closestObject != null && CompareTag("Choppable"))
            {
                if (PlayerState.Instance.playerBody.transform.Find("PlayerObj").transform.Find("ToolHolder").transform.Find("Axe_Model"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ChoppableTree choppableTree = GetComponent<ChoppableTree>();
                        choppableTree.GetHit();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Objenin collider'ina dokundugumuzda text olusturmali ve Range'inde oldugumuzu bildirmeli
        {
            playerInRange = true;

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
                else
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            objectsInRange.Remove(this);

            if (proximityText != null && (CompareTag("Collectible") || CompareTag("Killable")))
            {
                proximityText.gameObject.SetActive(false);
            }

            if (CompareTag("Choppable") || CompareTag("Killable") && entityInfoUI != null)
            {
                entityInfoUI.SetActive(false);
            }
        }
    }
}
