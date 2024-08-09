using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public GameObject dialogUI;
    public string npcName;
    public GameObject npcImage;
    public TextMeshProUGUI dialogText;
    public Button option1;
    public Button option2;
    public Button option3;
    public GameObject mainCanvas;

    public bool dialogUIActive;
    public static DialogSystem Instance { get; set; }
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
        dialogUIActive = false;
    }

    public void OpenDialogUI()
    {
        dialogUI.SetActive(true);
        mainCanvas.SetActive(false);
        dialogUIActive = true;
        Time.timeScale = 0;
        npcName = selectionManager.Instance.lastSelectedNPC.name;
        npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(npcName + "_CloseUp");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogUI()
    {
        dialogUI.SetActive(false);
        mainCanvas.SetActive(true);
        dialogUIActive = false;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
