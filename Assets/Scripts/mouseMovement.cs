using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yukarý aþaðý bakma mekaniði
        xRotation -= mouseY;

        // bakýþ açýsýnýn dönüþ sýnýrýný belirlemek için (sonsuza kadar yukarý veya aþaðý bakmamak için)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // saða sola bakma mekaniði
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
}
}
