using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selectionManager : MonoBehaviour // tamamen 1st person bakis acisinda yaptigimiz mouse mekanigiyle alakali bir script bu, eger mouse yoksa buna da gerek yok
{
    public GameObject interaction_info_UI;
    TextMeshProUGUI interaction_text;
    public static selectionManager Instance { get; set; }
    
    public bool onTarget;

    public GameObject selectedObject;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // gelecek kodlarda tamamen bunu birakabiliriz, tam su anda hic kullanmiyoruz, bos duruyor yani
        RaycastHit hit; // bir ustteki 37. satirdaki kodla birlikte, mouse'un uzerinde durdugu noktadan bir isin gonderiyor ve carptigi nesneyi ele aliyor
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform; // netlestirmek icin isinin denk geldigi objenin (hit) transform degerlerini selectionTransform diye yeniden adlandirip

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject; 
                interaction_text.text = interactable.GetItemName();
                interaction_info_UI.SetActive(true);
            }
            else //hit durumu var ama Interactable Object'e deðil
            {
                onTarget = false;
                interaction_info_UI.SetActive(false);
            }
        }
        else //hit durumu hiç yok, herhangi bir objeye bakmýyoruz
        {
            onTarget = false;
            interaction_info_UI.SetActive(false);
        }
    }
}
