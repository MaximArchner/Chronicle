using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    public float currentHealth, maxHealth;
    private ChoppableTree currentChoppableTree;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        EquipableItem equipableItem = GetComponent<EquipableItem>();
        if (equipableItem != null)
        {
            currentChoppableTree = equipableItem.currentChoppableTree;
        }
    }

    private void Update()
    {
        TextMeshProUGUI entityNameText = this.transform.parent.transform.Find("EntityName").GetComponent<TextMeshProUGUI>();
        if (entityNameText != null && entityNameText.text.Contains("PalmTree"))
        {
            currentHealth = currentChoppableTree.treeHealth;
            maxHealth = currentChoppableTree.treeMaxHealth;

            float fillValue = currentHealth / maxHealth; // orantisal olarak 0 ile 1 arasinda olacak (slider component icin)
            slider.value = fillValue;
        }
    }
}
