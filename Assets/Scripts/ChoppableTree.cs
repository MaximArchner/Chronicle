using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableTree : MonoBehaviour
{
    public GameObject toolHolder;
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    private void Update()
    {

        if (toolHolder.transform.Find("Axe_Model(Clone)"))
        {
            canBeChopped = true;
        }
        else
        {
            canBeChopped = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            canBeChopped = false;
        }
    }

    public void GetHit()
    {
        
        StartCoroutine(Hit());

    }

    public IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.2f);
        treeHealth -= 1;
    }
}
