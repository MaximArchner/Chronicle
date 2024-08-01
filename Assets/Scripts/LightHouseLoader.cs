using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LightHouseLoader : MonoBehaviour
{
    public bool playerInRange = false;
    public GameObject lighthouseText;
    public Transform player;

    private void Start()
    {
        lighthouseText = transform.Find("LightHouseText").gameObject;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("LighthouseInterior");
        }

        if (playerInRange)
        {
            lighthouseText.transform.position = lighthouseText.transform.parent.position + new Vector3(-1, 5, 0);
            lighthouseText.transform.LookAt(player.transform);
            lighthouseText.transform.Rotate(0, 180, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            lighthouseText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            lighthouseText.gameObject.SetActive(false);
        }
    }
}
