using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject selectedEntity = selectionManager.Instance.selectedEntity;

        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false)
        {

            animator.SetTrigger("hit");
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSound);
            if (selectedEntity != null)
            {
                if (selectedEntity.CompareTag("Choppable"))
                {
                    selectedEntity.GetComponent<ChoppableTree>().GetHit();
                }
                else if (selectedEntity.CompareTag("Killable"))
                {
                    selectedEntity.GetComponent<KillableRabbit>().GetHit();
                }
            }
        }
    }    
}
