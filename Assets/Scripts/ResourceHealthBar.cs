using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    public float currentHealth, maxHealth;
    private ChoppableTree currentChoppableTree;
    private KillableRabbit currentKillableRabbit;
    private InteractableObject interactableObject;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetCurrentChoppableTree(ChoppableTree tree)
    {
        currentChoppableTree = tree;
    }

    public void SetCurrentKillableRabbit(KillableRabbit rabbit)
    {
        currentKillableRabbit = rabbit;
    }

    public void SetInteractableObject(InteractableObject interactable)
    {
        interactableObject = interactable;
    }

    private void Update()
    {
        TextMeshProUGUI entityNameText = this.transform.parent.transform.Find("EntityName").GetComponent<TextMeshProUGUI>();
        GameObject entityInfoUI = this.transform.parent.gameObject;

        if (entityNameText != null && entityNameText.text.Contains("PalmTree"))
        {
            if (currentChoppableTree != null)
            {
                currentHealth = currentChoppableTree.treeHealth;
                maxHealth = currentChoppableTree.treeMaxHealth;

                float fillValue = currentHealth;
                this.transform.Find("HpText").GetComponent<TextMeshProUGUI>().text = fillValue + "/5";
                slider.value = fillValue;

                if (slider.value == 0)
                {
                    if(currentChoppableTree != null)
                    {
                        Destroy(currentChoppableTree.gameObject);
                        currentChoppableTree = null;
                        entityInfoUI.SetActive(false);
                    }

                    if (interactableObject != null)
                    {
                        interactableObject.ClosestObject = null;
                    }
                }
            }
        }
        else if (entityNameText != null && (entityNameText.text.Contains("Rabbit") || entityNameText.text.Contains("Big Rabbit")))
        {
            if (currentKillableRabbit != null)
            {
                currentHealth = currentKillableRabbit.rabbitHealth;
                maxHealth = currentKillableRabbit.rabbitHealth;

                float fillValue = currentHealth;
                this.transform.Find("HpText").GetComponent<TextMeshProUGUI>().text = fillValue + "/5";
                slider.value = fillValue;

                if (slider.value == 0)
                {
                    if(currentKillableRabbit != null)
                    {
                        Destroy(currentKillableRabbit.gameObject);
                        currentKillableRabbit = null;
                        entityInfoUI.SetActive(false);
                    }

                    if (interactableObject != null)
                    {
                        interactableObject.ClosestObject = null;
                    }
                }
            }
        }
    }
}
