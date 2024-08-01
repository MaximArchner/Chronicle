using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    public GameObject overrideWarning;
    Button proceedButton;
    Button cancelButton;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        proceedButton = overrideWarning.transform.Find("Confirm").GetComponent<Button>();
        cancelButton = overrideWarning.transform.Find("Cancel").GetComponent<Button>();
    }

    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                OverrideConfirmed();
            }
            else
            {
                DisplayOverrideWarning();
            }
        });
    }

    private void Update()
    {
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty File";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    private void DeselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void DisplayOverrideWarning()
    {
        overrideWarning.SetActive(true);

        proceedButton.onClick.AddListener(() =>
        {
            OverrideConfirmed();
            overrideWarning.SetActive(false);
        });

        cancelButton.onClick.AddListener(() =>
        {
            overrideWarning.SetActive(false);
        });
    }

    private void OverrideConfirmed()
    {
        SaveManager.Instance.SaveGame(slotNumber);
        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");

        string description = "Saved Game " + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);

        DeselectButton();
    }
}
