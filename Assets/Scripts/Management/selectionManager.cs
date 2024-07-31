using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectionManager : MonoBehaviour
{
    public static selectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject selectedEntity;
    public GameObject lastSelectedNPC;
    public Transform player;
    public float detectionRadius = 5f;
    public LayerMask detectionLayer;
    public GameObject entityInfoUI;
    public float selectedEntityHealth;
    public float selectedEntityMaxHealth;

    public List<string> removedEntities;

    private void Start()
    {
        onTarget = false;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.position, detectionRadius, detectionLayer);
        onTarget = false;

        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedEntity = interactable.gameObject;
                break;
            }
        }

        if (!onTarget)
        {
            selectedEntity = null;
        }

        if (selectedEntity != null && !selectedEntity.CompareTag("NPC"))
        {
            InteractableObject entityDetails = selectedEntity.GetComponent<InteractableObject>();

            entityInfoUI.SetActive(true);
            entityInfoUI.transform.Find("EntityName").GetComponent<TextMeshProUGUI>().text = entityDetails.ItemName;
            entityInfoUI.transform.Find("EntityDescription").GetComponent<TextMeshProUGUI>().text = entityDetails.entityDescription;
            entityInfoUI.transform.Find("EntityImage").GetComponent<Image>().sprite = entityDetails.entityImage;

            if (selectedEntity.CompareTag("Killable"))
            {
                float rabbitHealth = selectedEntity.GetComponent<KillableRabbit>().rabbitHealth;
                float rabbitMaxHealth = selectedEntity.GetComponent<KillableRabbit>().rabbitMaxHealth;
                selectedEntityHealth = rabbitHealth;
                selectedEntityMaxHealth = rabbitMaxHealth;
            }
            else if (selectedEntity.CompareTag("Choppable"))
            {
                float treeHealth = selectedEntity.GetComponent<ChoppableTree>().treeHealth;
                float treeMaxHealth = selectedEntity.GetComponent<ChoppableTree>().treeMaxHealth;
                selectedEntityHealth = treeHealth;
                selectedEntityMaxHealth = treeMaxHealth;
            }

            entityInfoUI.transform.Find("EntityHealth").transform.Find("HpText").GetComponent<TextMeshProUGUI>().text = selectedEntityHealth + "/" + selectedEntityMaxHealth;
            entityInfoUI.transform.Find("EntityHealth").GetComponent<Slider>().value = selectedEntityHealth;

            if (selectedEntityHealth <= 0 && !entityDetails.isBeingDestroyed)
            {
                entityDetails.isBeingDestroyed = true;
                if (!selectedEntity.name.Contains("PalmTree"))
                {
                    removedEntities.Add(selectedEntity.name);
                }
                else if (selectedEntity.name.Contains("PalmTree"))
                {
                    removedEntities.Add(selectedEntity.transform.parent.name);
                }
                StartCoroutine(DestroyObjectWithDelay(selectedEntity, entityDetails.entityInfoUI));
            }
        }
        else if(selectedEntity != null && selectedEntity.CompareTag("NPC"))
        {
            lastSelectedNPC = selectedEntity;
            InteractableObject entityDetails = selectedEntity.GetComponent<InteractableObject>();

            if (lastSelectedNPC.name.Contains("Samantha") && Input.GetKeyDown(KeyCode.F))
            {
                entityDetails.proximityText.text = lastSelectedNPC.name;
                entityDetails.ItemName = lastSelectedNPC.name;
            }
        }
    }

    IEnumerator DestroyObjectWithDelay(GameObject selectedEntity, GameObject entityInfoUI)
    {
        yield return new WaitForSeconds(0.2f);
        if (selectedEntity.CompareTag("Killable"))
        {
            Destroy(selectedEntity);
        }
        else if (selectedEntity.CompareTag("Choppable"))
        {
            Destroy(selectedEntity.transform.parent.gameObject);
        }
        entityInfoUI.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
