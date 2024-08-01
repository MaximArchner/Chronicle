using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer = false;
    private InteractableObject interactable;
    string npcName;
    public bool nameLearned;

    private void Start()
    {
        interactable = GetComponent<InteractableObject>();
        npcName = GetComponent<InteractableObject>().npcName;
    }

    private void Update()
    {
        playerInRange = interactable.playerInRange;
        
        if (this.gameObject.transform.name.Contains("Samantha"))
        {
            npcName = "Samantha";
            interactable.npcName = npcName;
        }

        if (playerInRange == true)
        {
            if (Input.GetKeyDown(KeyCode.F) && !isTalkingWithPlayer)
            {
                StartConversation();
                nameLearned = true;
            }
            else
            {
                isTalkingWithPlayer = false;
            }
        }
    }
    internal void StartConversation()
    {
        isTalkingWithPlayer = true;
    }
}
