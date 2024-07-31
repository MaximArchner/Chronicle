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

    public Animator _animator;

    public Transform treeEnvironment;

    private void Start()
    {
        treeHealth = treeMaxHealth;
        treeMaxHealth = 5;
        _animator = transform.parent.GetComponent<Animator>();
        treeEnvironment = this.transform.parent.transform.parent.GetComponent<Transform>();
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
        _animator.SetTrigger("Hit");
        treeHealth -= 1;

        if (treeHealth <= 0)
        {
            TreeIsChopped();
        }
    }

    void TreeIsChopped()
    {
        Vector3 treePosition = transform.position + new Vector3(0, 0, 0);
        canBeChopped = false;
        selectionManager.Instance.selectedEntity = null;

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            treePosition, Quaternion.Euler(0, 0, 0));

        RenameLogs(brokenTree);
        brokenTree.name = "ChoppedTree_" + LogCounter.GetNextLogNumber().ToString("D3");
        brokenTree.transform.SetParent(treeEnvironment);
        EnvironmentManager.Instance.entitiesAdded.Add(brokenTree.name);
    }

    void RenameLogs(GameObject choppedTree)
    {
        int logIndex = 1;
        foreach (Transform log in choppedTree.transform)
        {
            if (log.name.StartsWith("Log"))
            {
                log.name = "Log_" + LogCounter.GetNextLogNumber().ToString("D3");
                logIndex++;
            }
        }
    }
}
