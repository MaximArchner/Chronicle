using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ReturnHatch : MonoBehaviour
{
    public GameObject exitText;
    public bool playerInRange;
    public Transform lookToCamera;
    public Canvas loadingScreen;
    public Material clouds;

    private void Start()
    {
        exitText = transform.Find("ExitText").gameObject;
    }
    private void Update()
    {
        if (playerInRange && exitText != null)
        {
            exitText.transform.position = exitText.transform.parent.position + new Vector3(-4, 2, 0);
            exitText.transform.LookAt(lookToCamera);
            exitText.transform.Rotate(0, 180, 0);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            ActivateLoadingScreen();
            RenderSettings.skybox = clouds;
            SceneManager.LoadScene("Island");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            exitText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            exitText.SetActive(false);
        }
    }

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
