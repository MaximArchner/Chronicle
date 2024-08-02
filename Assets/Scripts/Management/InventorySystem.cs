using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public GameObject itemInfoUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject nextEmptySlot;
    
    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public UnityEngine.UI.Image pickupImage;

    public List<string> itemsPickedup;

    private Coroutine hidePickupAlertCoroutine;

    public int stackLimit = 99;

    public bool isOpen;

    //public bool isFull; sonra kullanirsak diye saklayalim

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


    void Start()
    {
        isOpen = false;

        PopulateSlotList();
        ReCalculateList();

    }

    private void PopulateSlotList() // Envanter haznelerini sayip durumlarini sonradan degistirip kontrol edebilmemiz icin bir listeye esitleyecek
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !MenuManager.Instance.isMenuOpen) // Envanteri acma
        {
            inventoryScreenUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            isOpen = true;
            ReCalculateList();
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen && !MenuManager.Instance.isMenuOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (CraftingSystem.Instance.isOpen == false)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName, bool shouldStack) // spesifik bir objeyi envanter listesine ekleyebilme metodu
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.pickItemSound);

        GameObject stack = CheckIfStackExists(itemName);

        if (stack != null && shouldStack)
        {
            InventorySlot slot = stack.GetComponent<InventorySlot>();
            if (slot != null && slot.itemInSlot != null)
            {
                slot.itemInSlot.amountInInventory++;
                slot.UpdateItemInSlot();
            }
        }
        else
        {
            nextEmptySlot = FindNextEmptySlot();
            if (nextEmptySlot != null)
            {
                Debug.Log("Next empty slot found: " + nextEmptySlot.name);
                Debug.Log("Attempting to load item from Resources: " + itemName);
                GameObject itemPrefab = Resources.Load<GameObject>(itemName);
                if (itemPrefab != null)
                {
                    Debug.Log("Item loaded from Resources: " + itemPrefab.name);
                    itemToAdd = Instantiate(itemPrefab, nextEmptySlot.transform.position, nextEmptySlot.transform.rotation);
                    if (itemToAdd != null)
                    {
                        itemToAdd.transform.SetParent(nextEmptySlot.transform);
                        itemList.Add(itemName);
                    }
                }
                else
                {
                    Debug.LogError("Failed to load item from Resources: " + itemName);
                }
            }
            else
            {
                Debug.LogError("No empty slot found!");
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        if (itemToAdd != null)
        {
            TriggerPickupPopUp(itemName, itemToAdd.GetComponent<UnityEngine.UI.Image>().sprite);
        }
        else
        {
            Debug.LogError("itemToAdd is null after attempting to add to inventory.");
        }
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        if (hidePickupAlertCoroutine != null)
        {
            StopCoroutine(hidePickupAlertCoroutine);
        }

        hidePickupAlertCoroutine = StartCoroutine(HidePickupAlertAfterDelay(3f));
    }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        pickupAlert.SetActive(false);
        hidePickupAlertCoroutine = null;
    }
    private GameObject FindNextEmptySlot() // envanterde yer varsa, toplanan bir objeyi mevcut bos yere koyuyor
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        foreach (GameObject slot in slotList)
        {
            if (remainingAmountToRemove == 0)
            {
                break;
            }

            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot.itemInSlot != null && inventorySlot.itemInSlot.thisName == itemName)
            {
                while (inventorySlot.itemInSlot.amountInInventory > 0 && remainingAmountToRemove > 0)
                {
                    inventorySlot.itemInSlot.amountInInventory--;
                    remainingAmountToRemove--;

                    if (inventorySlot.itemInSlot.amountInInventory == 0)
                    {
                        Destroy(inventorySlot.itemInSlot.gameObject);
                        inventorySlot.itemInSlot = null;
                        break;
                    }
                }
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void ReCalculateList()
    {

        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            InventoryItem item = slot.GetComponent<InventorySlot>().itemInSlot;

                if (item != null)
                {
                    if (item.amountInInventory > 0)
                    {
                        for (int i = 0; i < item.amountInInventory; i++)
                        {
                            itemList.Add(item.thisName);
                        }
                    }
                }
        }
    }

    private GameObject CheckIfStackExists(string ItemName)
    {
        foreach (GameObject slot in slotList)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();

            inventorySlot.UpdateItemInSlot();
            
            if(inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == ItemName 
                    && inventorySlot.itemInSlot.amountInInventory < stackLimit)
                {
                    return slot;
                }
            }
        }

        return null;
    }

    public bool CheckSlotsAvailable(int emptyMeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount <= 1)
            {
                emptySlot++;
            }
        }

        if (emptySlot >= emptyMeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}