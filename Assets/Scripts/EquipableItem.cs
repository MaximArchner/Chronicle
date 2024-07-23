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

        if (Input.GetMouseButtonDown(0)  // 0 sol týk
            && InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false)
        {
            animator.SetTrigger("hit");
        }

    }
}
