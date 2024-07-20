using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System.Runtime.CompilerServices;

public class QuickSlotsSystem : MonoBehaviour
{
    public static QuickSlotsSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    //public List<string> itemList = new List<string>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;
    public GameObject selectedItemModel;

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


                // daha önce seçilen itemi seçmeme
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquippedModel(selectedItem);


                //rengi deðiþtirmek için

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("SlotNumber").GetComponent<TextMeshProUGUI>().color = Color.gray;
                }

                TextMeshProUGUI toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("SlotNumber").GetComponent<TextMeshProUGUI>();
                toBeChanged.color = Color.white;
            }
            else // Ayný slotu seçmeyi deniyoruz
            {
                selectedNumber = -1; //boþ anlamýna gelir

                // daha önce seçilen itemi seçmeme
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



                //rengi deðiþtirmek için

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

            if (quickSlotsList[slotnumber-1].transform.childCount > 1)
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
            new Vector3(0.6f, 0, 0.4f), Quaternion.Euler(0, 49.71f, -21f)); //aletin konumunu deðiþtireceðimiz satýr//
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
        // Sýradaki Boþ Slotu bulma
        GameObject availableSlot = FindNextEmptySlot();
        // Objemizi transform etme
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Temiz(yeni) isim alma
        //string cleanName = itemToEquip.name.Replace("(Clone)", "");
        //itemList.Add(cleanName);
        InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
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
            if (slot.transform.childCount > 0)
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
