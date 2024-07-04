using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingMainScreenUI;
    public GameObject craftingToolsScreenUI;

    public List<string> inventoryItemList = new List<string>();


    //Category Buttons Ref
    Button toolsButton;

    //Craft Buttons Ref
    Button craftAxeButton;

    //Req Text
    Text AxeReq1, AxeReq2;

    public bool isOpen;

    //All BP


    public static CraftingSystem Instance{ get; set; }
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




    // Start is called before the first frame update
    void Start()
    {

        isOpen = false;

        toolsButton = craftingMainScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsCategory(); });

        //Axe
        AxeReq1 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = craftingToolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = craftingToolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(); });
    }
    void OpenToolsCategory()
    {
        craftingMainScreenUI.SetActive(false);
        craftingToolsScreenUI.SetActive(true);
    }


    void CraftAnyItem()
    {

        //Envantere item ekleme


        //Envanterden kaynak silme



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen) // Envanteri acma
        {

            craftingMainScreenUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingMainScreenUI.SetActive(false);
            craftingToolsScreenUI.SetActive(false);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            isOpen = false;
        }
    }
}
