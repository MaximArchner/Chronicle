using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    //public Transform cameraTransform;
    //public Cinemachine.CinemachineFreeLook cinemachineFreeLook;

    public float rotationSpeed;
    public float focusDuration = 1.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //if (cameraTransform == null)
        //{
        //    cameraTransform = Camera.main.transform;
        //}

        //if (cinemachineFreeLook == null)
        //{
        //    cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        //}
    }

    private void Update()
    {
            // rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            // rotate player object
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
    //public void FocusOnNPC(Transform npcTransform)
    //{
    //    StartCoroutine(FocusRoutine(npcTransform));
    //}

    //private IEnumerator FocusRoutine(Transform npcTransform)
    //{
    //    float elapsedTime = 0.0f;
    //    Vector3 initialPosition = cameraTransform.position;
    //    Quaternion initialRotation = cameraTransform.rotation;

    //    Vector3 targetPosition = npcTransform.position + npcTransform.forward * -2.0f + Vector3.up * 1.5f;
    //    Quaternion targetRotation = Quaternion.LookRotation(npcTransform.position - targetPosition);

    //    while (elapsedTime < focusDuration)
    //    {
    //        cameraTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / focusDuration);
    //        cameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / focusDuration);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    cameraTransform.position = targetPosition;
    //    cameraTransform.rotation = targetRotation;
    //}
}
