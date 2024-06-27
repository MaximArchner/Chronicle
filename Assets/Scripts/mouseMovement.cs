using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;
    public float groundDistance = 0.8f;

    public Transform groundCheck;
    Vector3 initialGroundCheckLocalPosition;
    Vector3 groundCheckOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        groundCheckOffset = groundCheck.position - transform.position;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yukarý aþaðý bakma mekaniði
        xRotation -= mouseY;

        // bakýþ açýsýnýn dönüþ sýnýrýný belirlemek için (sonsuza kadar yukarý veya aþaðý bakmamak için)
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        // saða sola bakma mekaniði
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        AdjustGroundCheckPosition();
    }

    void AdjustGroundCheckPosition()
    {
        Vector3 adjustedPosition = transform.position + transform.rotation * initialGroundCheckLocalPosition;
        groundCheck.position = adjustedPosition;
    }
}
