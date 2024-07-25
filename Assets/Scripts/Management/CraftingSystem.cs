using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingMainScreenUI;
    public GameObject craftingToolsScreenUI;


    public List<string> inventoryItemList = new List<string>();

    // Kategori Buttonu
    Button toolsButton;

    // Oluþtur Buttonu
    Button craftAxeButton;

    // Gereklilik Yazýsý
    TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;

    // All BP
    public ItemBP AxeBP = new ItemBP("Axe", 2, "Stone", 3, "Stick", 3 );

    public static CraftingSystem Instance { get; set; }
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

        toolsButton = craftingMainScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsCategory(); });

        // Balta
        AxeReq1 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();

        craftAxeButton = craftingToolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBP); });
    }

    void OpenToolsCategory()
    {
        craftingMainScreenUI.SetActive(false);
        craftingToolsScreenUI.SetActive(true);
    }

    void CraftAnyItem(ItemBP blueprintToCraft)
    {
        // Envantere item ekleme
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, false);

        // Envanterden kaynak silme
        if (blueprintToCraft.numOfReq == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfReq == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        // Listeyi güncelle
        StartCoroutine(Calculate());
    }

    public IEnumerator Calculate()
    {

        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    /*IENumerator CraftedDelayForSound(ItemBP blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        for (var i  = 0; i < blueprintToCraft.numOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, true);
        }
    }*/

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !MenuManager.Instance.isMenuOpen) // Envanteri açma
        {
            craftingMainScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;
            RefreshNeededItems();
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen && !MenuManager.Instance.isMenuOpen)
        {
            craftingMainScreenUI.SetActive(false);
            craftingToolsScreenUI.SetActive(false);

            if (InventorySystem.Instance.isOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch(itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        // BALTA
        AxeReq1.text = "3 Stone[" + stone_count + "]";
        AxeReq2.text = "3 Stick[" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3) 
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }
    }
}
