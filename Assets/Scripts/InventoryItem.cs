using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool isTrashable;

    // --- ItemINFO UI --- //
    private GameObject itemInfoUI;

    private TextMeshProUGUI itemInfoUI_itemName;
    private TextMeshProUGUI itemInfoUI_itemDescription;
    private TextMeshProUGUI itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // Esya tuketme
    private GameObject itemPendingConsumption;
    public bool isConsumable;
    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

 
    public int amountInInventory = 1;

    public bool isSelected;
    public bool isUseable;

    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;
    
    
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<TextMeshProUGUI>();
    }

    void Update ()
    { 

        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent <DragDrop>().enabled = true;
        }

    }

    // hover'ladigimizda bu event calisacak
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    // mouse hover'dan cikarsa bu olacak
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }
    
    // mouse ile tiklarsak bu calisacak
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, hungerEffect, thirstEffect);
            }

            if (isEquippable && isInsideQuickSlot == false && QuickSlotsSystem.Instance.CheckIfFull() == false)
            {

                QuickSlotsSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;

            }
        }



    }

    public void OnPointerUp(PointerEventData eventData)
    {
               
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                amountInInventory--;
                if (amountInInventory <= 0)
                {
                    DestroyImmediate(gameObject);
                }
            }
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }
    }

    private void consumingFunction(float healthEffect, float hungerEffect, float thirstEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);
        hungerEffectCalculation(hungerEffect);
        thirstEffectCalculation(thirstEffect);
    }

    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Can Etkisi Hesaplama --- //
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }
    private static void hungerEffectCalculation(float hungerEffect)
    {
        float hungerBeforeConsumption = PlayerState.Instance.currentHunger;
        float maxHunger = PlayerState.Instance.maxHunger;

        if (hungerEffect != 0)
        {
            if ((hungerBeforeConsumption + hungerEffect) > maxHunger)
            {
                PlayerState.Instance.setHunger(maxHunger);
            }
            else
            {
                PlayerState.Instance.setHunger(hungerBeforeConsumption + hungerEffect);
            }
        }
    }
    private static void thirstEffectCalculation(float thirstEffect)
    {
        float thirstBeforeConsumption = PlayerState.Instance.currentThirstPercent;
        float maxThirst = PlayerState.Instance.maxThirstPercent;

        if (thirstEffect != 0)
        {
            if ((thirstBeforeConsumption + thirstEffect) > maxThirst)
            {
                PlayerState.Instance.setThirst(maxThirst);
            }
            else
            {
                PlayerState.Instance.setThirst(thirstBeforeConsumption + thirstEffect);
            }
        }
    }
}
