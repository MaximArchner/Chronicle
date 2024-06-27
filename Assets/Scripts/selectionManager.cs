using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectionManager : MonoBehaviour
{
    public GameObject interaction_info_UI;
    TextMeshProUGUI interaction_text;

    private void Start()
    {
        interaction_text = interaction_info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject >())
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_info_UI.SetActive(true);
            }
            else //hit durumu var ama Interactable Object'e deðil
            {
                interaction_info_UI.SetActive(false);
            }
        }
        else //hit durumu hiç yok, herhangi bir objeye bakmýyoruz
        {
            interaction_info_UI.SetActive(false);
        }
    }
}
