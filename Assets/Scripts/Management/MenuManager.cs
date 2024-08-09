using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject MenuCanvas;
    public GameObject otherCanvas;
    public GameObject mapCanvas;

    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public Button ResumeButton;

    public bool isMenuOpen;
    private bool resumeButtonClicked;
    
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
        isMenuOpen = false;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P) && !isMenuOpen)
        {
            if (otherCanvas != null)
            {
                otherCanvas.SetActive(false);
            }
            if (mapCanvas != null)
            {
                mapCanvas.SetActive(false);
                PlayerState.Instance.mainMapIsOpen = false;
            }
            MenuCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isMenuOpen = true;
        }
        else if ((Input.GetKeyDown(KeyCode.P) || resumeButtonClicked) && isMenuOpen)
        {
            if (DialogSystem.Instance.dialogUIActive == false)
            {
                otherCanvas.SetActive(true);
            }
            MenuCanvas.SetActive(false);

            saveMenu.SetActive(false);
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);

            if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false && QuestManager.Instance.isQuestMenuOpen == false && DialogSystem.Instance.dialogUIActive == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            Time.timeScale = 1f;
            isMenuOpen = false;
            resumeButtonClicked = false;
            DeselectButton();
        }
    }

    public void ResumeGame()
    {
        resumeButtonClicked = true;
        DeselectButton();
    }

    private void DeselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
