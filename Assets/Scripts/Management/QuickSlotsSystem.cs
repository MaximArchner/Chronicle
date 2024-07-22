using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlotsSystem : MonoBehaviour
{
    public static QuickSlotsSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;
    public GameObject numbersHolder;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;
    public GameObject selectedItemModel;
    public Vector3 ModelPosition;
    public Quaternion ModelRotation;

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


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            SelectQuickSlot(1);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);

        }

    }


    void SelectQuickSlot(int number)
    {
        if (CheckIfSlotFull(number) == true)
        {
            if (selectedNumber != number)
            {

                selectedNumber = number;


                // daha �nce se�ilen itemi se�meme
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquippedModel(selectedItem);


                //rengi degistirmek icin

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("SlotNumber").GetComponent<TextMeshProUGUI>().color = Color.gray;
                }

                TextMeshProUGUI toBeChanged = numbersHolder.transform.Find("Number" + number).transform.Find("SlotNumber").GetComponent<TextMeshProUGUI>();
                toBeChanged.color = Color.white;
            }
            else // Ayni slotu secmeyi deniyoruz
            {
                selectedNumber = -1; //bos anlamina gelir

                // daha once secilen itemi secmeme
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }
                if (selectedItemModel != null)
                {

                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;

                }


                //rengi degistirmek icin

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("SlotNumber").GetComponent<TextMeshProUGUI>().color = Color.gray;
                }

            }
        }


        GameObject GetSelectedItem(int slotNumber)
        {

            return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;

        }

        bool CheckIfSlotFull(int slotnumber)
        {

            if (quickSlotsList[slotnumber-1].transform.childCount > 0)
            {

                return true;

            }
            else
            {
                return false;
            }

        }


    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {

            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;

        }

        string selectedItemName = selectedItem.name.Replace("(Clone)","");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
            ModelPosition, ModelRotation); //aletin konumunu de�i�tirece�imiz sat�r//
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // S�radaki Bo� Slotu bulma
        GameObject availableSlot = FindNextEmptySlot();
        // Objemizi transform etme
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 1)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 1)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
