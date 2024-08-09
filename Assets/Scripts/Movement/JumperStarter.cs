using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperStarter : MonoBehaviour
{
    private playerMovement playerMovementScript;

    private void Start()
    {
        playerMovementScript = GetComponentInParent<playerMovement>();
    }

    public void TriggerJump()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.Jump();
        }
    }
}
