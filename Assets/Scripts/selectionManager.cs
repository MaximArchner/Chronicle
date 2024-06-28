using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectionManager : MonoBehaviour
{
    public GameObject interaction_info_UI;
    TextMeshProUGUI interaction_text;
    public static selectionManager Instance { get; set; }
    public bool onTarget;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_info_UI.GetComponent<TextMeshProUGUI>();
    }

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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject >() && selectionTransform.GetComponent<InteractableObject>().playerInRange)
            {
                onTarget = true;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_info_UI.SetActive(true);
            }
            else //hit durumu var ama Interactable Object'e de�il
            {
                onTarget = false;
                interaction_info_UI.SetActive(false);
            }
        }
        else //hit durumu hi� yok, herhangi bir objeye bakm�yoruz
        {
            onTarget = false;
            interaction_info_UI.SetActive(false);
        }
    }
}
