using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingMainScreenUI;
    public GameObject craftingToolsScreenUI, survivalScreenUI, refineScreenUI;


    public List<string> inventoryItemList = new List<string>();

    // Kategori Buttonu
    Button toolsButton, survivalButton, refineButton;

    // Olu�tur Buttonu
    Button craftAxeButton, craftPlankButton;

    // Gereklilik Yaz�s�
    TextMeshProUGUI AxeReq1, AxeReq2, PlankReq1;

    public bool isOpen;

    // All BP
    public ItemBP AxeBP = new ItemBP("Axe", 1, 2, "Stone", 3, "Stick", 3 );
    public ItemBP PlankBP = new ItemBP("Plank", 2, 1, "Log", 1, "", 0);




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

        survivalButton = craftingMainScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalButton.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineButton = craftingMainScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineButton.onClick.AddListener(delegate { OpenRefineCategory(); });

        // Balta
        AxeReq1 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();

        craftAxeButton = craftingToolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBP); });

        // Tahta
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<TextMeshProUGUI>();

        craftPlankButton = refineScreenUI.transform.Find("Plank").transform.Find("CraftButton").GetComponent<Button>();
        craftPlankButton.onClick.AddListener(delegate { CraftAnyItem(PlankBP); });
    }
    void OpenToolsCategory()
    {
        craftingMainScreenUI.SetActive(false);

        craftingToolsScreenUI.SetActive(true);

        refineScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
    }
    void OpenSurvivalCategory()
    {
        craftingMainScreenUI.SetActive(false);
        craftingToolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);

    }
    void OpenRefineCategory()
    {
        craftingMainScreenUI.SetActive(false);
        craftingToolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }
    void CraftAnyItem(ItemBP blueprintToCraft)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        for (var i = 0; i < blueprintToCraft.numberOfItemsProduce; i ++)
        {
            // Envantere item ekleme
           if(blueprintToCraft == AxeBP) // Eger craftlanan item bir aletse
            {
                InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, false);
            }
           else if(blueprintToCraft == PlankBP) // Eger craftlanan item bir materyalse
            {
                InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, true);
            }
        }

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

        // Listeyi g�ncelle
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

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !MenuManager.Instance.isMenuOpen) // Envanteri a�ma
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
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);



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
        int log_count = 0;

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
                
                case "Log":
                    log_count += 1;
                    break;
            }
        }

        // BALTA
        AxeReq1.text = "3 Stone[" + stone_count + "]";
        AxeReq2.text = "3 Stick[" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1)) 
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }


        // Tahta x2
        PlankReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankButton.gameObject.SetActive(true);
        }
        else
        {
            craftPlankButton.gameObject.SetActive(false);
        }

    }
}
