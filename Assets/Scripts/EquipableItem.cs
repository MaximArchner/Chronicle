using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)  // 0 sol týk
            && InventorySystem.Instance.isOpen == false && CraftingSystem.Instance.isOpen == false)
        {
            animator.SetTrigger("hit");
        }

    }
}
