using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private MonoBehaviour cameraController; 

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartConversation();
            }
        }
    }

    private void StartConversation()
    {
        if (cameraController != null)
        {
            cameraController.enabled = false; 
        }

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 

        ConversationManager.Instance.StartConversation(myConversation);

        ConversationManager.OnConversationEnded += EndConversation;
    }

    private void EndConversation()
    {
        if (cameraController != null)
        {
            cameraController.enabled = true; 
        }

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 

        ConversationManager.OnConversationEnded -= EndConversation;
    }
}
