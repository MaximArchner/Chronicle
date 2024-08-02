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

    public List<InventorySlot> slotList = new List<InventorySlot>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private InventorySlot nextEmptySlot;
    
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
                InventorySlot slot = child.GetComponent<InventorySlot>();
                slotList.Add(slot);
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
        if (itemToAdd != null)
        {
            TriggerPickupPopUp(itemName, itemToAdd.GetComponent<UnityEngine.UI.Image>().sprite);
        }

        InventorySlot stack = CheckIfStackExists(itemName);

        if (stack != null && shouldStack)
        {
            InventorySlot slot = stack.GetComponent<InventorySlot>();
            stack.itemInSlot.amountInInventory += 1;
            stack.UpdateItemInSlot();
        }
        else
        {
            nextEmptySlot = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), nextEmptySlot.transform.position, nextEmptySlot.transform.rotation);
            itemToAdd.transform.SetParent(nextEmptySlot.transform);
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        QuestManager.Instance.RefreshTrackerList();
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
    private InventorySlot FindNextEmptySlot() // envanterde yer varsa, toplanan bir objeyi mevcut bos yere koyuyor
    {
        foreach (InventorySlot slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }

        return new InventorySlot();
    }

    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        while (remainingAmountToRemove != 0)
        {
            int previousRemainingAmount = remainingAmountToRemove;

            foreach (InventorySlot slot in slotList)
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

            if (previousRemainingAmount == remainingAmountToRemove)
            {
                Debug.Log("Item not found or insufficient quantity in inventory.");
                break;
            }

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            QuestManager.Instance.RefreshTrackerList();
        }
    }

    public void ReCalculateList()
    {

        itemList.Clear();
        foreach (InventorySlot inventorySlot in slotList)
        {
            InventoryItem item = inventorySlot.itemInSlot;

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

    private InventorySlot CheckIfStackExists(string ItemName)
    {
        foreach (InventorySlot inventorySlot in slotList)
        {
            inventorySlot.UpdateItemInSlot();
            
            if(inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == ItemName 
                    && inventorySlot.itemInSlot.amountInInventory < stackLimit)
                {
                    return inventorySlot;
                }
            }
        }

        return null;
    }

    public bool CheckSlotsAvailable(int emptyMeeded)
    {
        int emptySlot = 0;

        foreach (InventorySlot slot in slotList)
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

    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;

        foreach(string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }
        }

        return itemCounter;
    }
}