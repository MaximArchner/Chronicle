using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    public bool playerInRange;
    public bool hasPlayerGotFuel = false;
    public GameObject mainCanvas;
    public GameObject endImage;
    public TextMeshPro exitText;
    public GameObject player;
    public Vector3 offSet;

    public void Start()
    {
        offSet = new Vector3(0, 10, 10);
    }
    private void Update()
    {
        if (InventorySystem.Instance.itemList.Contains("Fuel"))
        {
            hasPlayerGotFuel = true;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.J) && hasPlayerGotFuel)
        {
            endImage.SetActive(true);
            mainCanvas.SetActive(false);
            StartCoroutine(WaitAndReturnToMainMenu());
        }
        
        if (playerInRange)
        {
            exitText.transform.position = exitText.transform.parent.position + offSet;
            exitText.transform.LookAt(player.transform);
            exitText.transform.Rotate(0, 180, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasPlayerGotFuel)
        {
            playerInRange = true;
            exitText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            exitText.gameObject.SetActive(false);
        }   
    }

    private IEnumerator WaitAndReturnToMainMenu()
    {
        yield return new WaitForSeconds(2f);
        SaveManager.Instance.SaveGame(0);
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
