using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Ink.Parsed;

public class LightHouseLoader : MonoBehaviour
{
    public bool playerInRange = false;
    public GameObject lighthouseText;
    public Transform player;
    public List<string> inventoryList;

    private void Start()
    {
        lighthouseText = transform.Find("LightHouseText").gameObject;
        inventoryList = InventorySystem.Instance.itemList;
        InventorySystem.Instance.ReCalculateList();
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L))
        {
            if (CheckIfKeyExist())
            {
                SaveManager.Instance.SaveGame(0);
                SceneManager.LoadScene("LighthouseInterior");
            }
        }

        if (playerInRange)
        {
            lighthouseText.transform.position = lighthouseText.transform.parent.position + new Vector3(-1, 4, 0);
            lighthouseText.transform.LookAt(player.transform);
            lighthouseText.transform.Rotate(0, 180, 0);
        }
    }

    private bool CheckIfKeyExist()
    {
        InventorySystem.Instance.ReCalculateList();
        return inventoryList.Contains("Lighthouse Key");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            lighthouseText.SetActive(true);
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
