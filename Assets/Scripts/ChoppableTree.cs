using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public GameObject toolHolder;
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    private void Start()
    {
        treeHealth = treeMaxHealth;
        treeMaxHealth = 5;
    }
    private void Update()
    {
        if (toolHolder.transform.Find("Axe_Model(Clone)") && playerInRange == true)
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
        yield return new WaitForSeconds(0.4f);
        treeHealth -= 1;
    }
}
