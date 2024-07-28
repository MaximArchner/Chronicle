using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    public ChoppableTree currentChoppableTree;
    public KillableRabbit currentKillableRabbit;
    public float detectionRadius = Mathf.Infinity;
    public LayerMask treeLayerMask;
    public LayerMask animalLayerMask;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false)
        {
            animator.SetTrigger("hit");

            if (currentChoppableTree != null)
            {
                currentChoppableTree.GetHit();
            }
            
            if (currentKillableRabbit != null)
            {
                currentKillableRabbit.GetHit();
            }
        }

        UpdateClosestTree();
        UpdateClosestRabbit();
    }

    void UpdateClosestTree()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, treeLayerMask);
        float closestDistance = detectionRadius;
        ChoppableTree closestTree = null;

        foreach (var hitCollider in hitColliders)
        {
            ChoppableTree tree = hitCollider.GetComponent<ChoppableTree>();
            if (tree != null)
            {
                float distance = Vector3.Distance(transform.position, tree.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTree = tree;
                }
            }
        }

        currentChoppableTree = closestTree;
        UpdateResourceHealthBar();
    }

    void UpdateClosestRabbit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, animalLayerMask);
        float closestDistance = detectionRadius;
        KillableRabbit closestRabbit = null;

        foreach (var hitCollider in hitColliders)
        {
            KillableRabbit rabbit = hitCollider.GetComponent<KillableRabbit>();
            if (rabbit != null)
            {
                float distance = Vector3.Distance(transform.position, rabbit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRabbit = rabbit;
                }
            }
        }

        currentKillableRabbit = closestRabbit;
        UpdateResourceHealthBar();
    }

    void UpdateResourceHealthBar()
    {
        ResourceHealthBar resourceHealthBar = FindObjectOfType<ResourceHealthBar>();
        if (resourceHealthBar != null)
        {
            resourceHealthBar.SetCurrentChoppableTree(currentChoppableTree);
            resourceHealthBar.SetCurrentKillableRabbit(currentKillableRabbit);

            if (currentChoppableTree != null)
            {
                InteractableObject interactableObject = currentChoppableTree.GetComponent<InteractableObject>();
                resourceHealthBar.SetInteractableObject(interactableObject);
            }
            else
            {
                resourceHealthBar.SetInteractableObject(null);
            }
        }
    }
}
