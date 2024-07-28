using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class KillableRabbit : MonoBehaviour
{
    public GameObject toolHolder;
    public bool playerInRange;
    public bool canBeKilled;

    public float rabbitMaxHealth;
    public float rabbitHealth;

    private void Start()
    {
        rabbitHealth = rabbitMaxHealth;
        rabbitMaxHealth = 5;
    }
    private void Update()
    {
        if (toolHolder.transform.Find("Axe_Model(Clone)") && playerInRange == true)
        {
            canBeKilled = true;
        }
        else
        {
            canBeKilled = false;
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
            canBeKilled = false;
        }
    }

    public void GetHit()
    {
        StartCoroutine(Hit());
    }

    public IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.2f);
        rabbitHealth -= 1;
    }
}
