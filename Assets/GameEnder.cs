using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    public bool playerInRange;
    public bool hasPlayerGotFuel;
    public GameObject mainCanvas;
    public GameObject endImage;

    private void Update()
    {
        if (playerInRange && InventorySystem.Instance.itemList.Contains("Fuel") && Input.GetKeyDown(KeyCode.J))
        {
            endImage.SetActive(true);
            mainCanvas.SetActive(false);
            StartCoroutine(WaitAndReturnToMainMenu());
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

    private IEnumerator WaitAndReturnToMainMenu()
    {
        yield return new WaitForSeconds(2f);
        SaveManager.Instance.SaveGame(0);
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
