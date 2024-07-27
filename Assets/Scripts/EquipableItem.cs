using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{

    public Animator animator;
    public ChoppableTree currentChoppableTree;
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

            if (currentChoppableTree != null)
            {
                currentChoppableTree.GetHit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Choppable"))
        {
            currentChoppableTree = other.GetComponent<ChoppableTree>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Choppable"))
        {
            currentChoppableTree = null;
        }
    }
}
