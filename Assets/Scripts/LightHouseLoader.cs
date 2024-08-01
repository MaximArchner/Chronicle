using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightHouseLoader : MonoBehaviour
{
    public bool playerInRange = false;
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("LighthouseInterior");
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
        }
    }
}
