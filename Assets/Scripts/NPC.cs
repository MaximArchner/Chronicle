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
    //private ThirdPersonCam thirdPCam;

    private void Start()
    {
        interactable = GetComponent<InteractableObject>();
        npcName = GetComponent<InteractableObject>().npcName; 
        //GameObject thirdPersonCamObj = GameObject.FindWithTag("ThirdPersonCam");
        //if (thirdPersonCamObj != null)
        //{
        //    thirdPCam = thirdPersonCamObj.GetComponent<ThirdPersonCam>();
        //    if (thirdPCam == null)
        //    {
        //        Debug.LogError("ThirdPersonCam component not found on the ThirdPersonCam object.");
        //    }
        //}
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
        //thirdPCam.FocusOnNPC(this.transform);
    }
}
