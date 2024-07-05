using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject nextEmptySlot;

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

        if (Input.GetKeyDown(KeyCode.I) && !isOpen) // Envanteri acma
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
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

    public void AddToInventory(string itemName) // spesifik bir objeyi envanter listesine ekleyebilme metodu
    {
            nextEmptySlot = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), nextEmptySlot.transform.position, nextEmptySlot.transform.rotation);
            itemToAdd.transform.SetParent(nextEmptySlot.transform);
            itemList.Add(itemName);
    }
    private GameObject FindNextEmptySlot() // envanterde yer varsa, toplanan bir objeyi mevcut bos yere koyuyor
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public bool CheckIfFull() // envanterde bos yuva var mi bakacak, yoksa False dondurecek
    {
        int counter = 0;
        foreach (GameObject slot in slotList) 
        {
            if (slot.transform.childCount > 0) 
            {
                counter += 1;
            }
        }

        if (counter == 21)
        {
            return true;
        }

        else
        {
            return false;
        }
    }


    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {

            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {

                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;


                }



            }



        } 



    }
    public void ReCalculateList()
    {

        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {

                string name = slot.transform.GetChild(0).name; //Taþ (klonu)

                string str2 = "(Clone)";

                string result = name.Replace(str2, "");


                itemList.Add(result);

            }

        }





    }

}