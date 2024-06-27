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

        // yukar� a�a�� bakma mekani�i
        xRotation -= mouseY;

        // bak�� a��s�n�n d�n�� s�n�r�n� belirlemek i�in (sonsuza kadar yukar� veya a�a�� bakmamak i�in)
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        // sa�a sola bakma mekani�i
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
