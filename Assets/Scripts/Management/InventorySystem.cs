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

        UnityEngine.Cursor.visible = false;
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

            //Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            selectionManager.Instance.DisableSelection();
            selectionManager.Instance.GetComponent<selectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen && !MenuManager.Instance.isMenuOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (CraftingSystem.Instance.isOpen == false)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                selectionManager.Instance.DisableSelection();
                selectionManager.Instance.GetComponent<selectionManager>().enabled = true;
            }

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName, bool shouldStack) // spesifik bir objeyi envanter listesine ekleyebilme metodu
    {
        GameObject stack = CheckIfStackExists(itemName);

        // if(SaveManager.Instance.isLoading == false)
        // {
        // SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound());
        // }


        if (stack != null && shouldStack)
        {
            Debug.Log("Stack exists with this item: " + itemName);
            stack.GetComponent<InventorySlot>().itemInSlot.amountInInventory++;
            stack.GetComponent<InventorySlot>().UpdateItemInSlot();
        }
        else
        {
            nextEmptySlot = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), nextEmptySlot.transform.position, nextEmptySlot.transform.rotation);
            itemToAdd.transform.SetParent(nextEmptySlot.transform);
            itemList.Add(itemName);
        }
        
        
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<UnityEngine.UI.Image>().sprite);
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

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {

            if (slotList[i].transform.childCount > 0)
            {
                InventorySlot slot  = slotList[i].GetComponent<InventorySlot>();
                if (slot != null && slot.itemInSlot != null && slot.itemInSlot.thisName == nameToRemove && counter > 0)
                {
                    if(slot.itemInSlot.amountInInventory > counter)
                    {
                        slot.itemInSlot.amountInInventory -= counter;
                        slot.UpdateItemInSlot();
                        counter = 0;
                    }
                    else
                    {
                        counter -= slot.itemInSlot.amountInInventory;
                        Destroy(slot.transform.GetChild(0).gameObject);
                    }
                }
            }
            
            if (counter == 0)
            {
                break;
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
            if (slot.GetComponent<InventorySlot>())
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
    }

    private GameObject CheckIfStackExists(string ItemName)
    {
        foreach (GameObject slot in slotList)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            inventorySlot.UpdateItemInSlot();
            if(inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == ItemName && inventorySlot.itemInSlot.amountInInventory < stackLimit)
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