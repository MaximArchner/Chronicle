using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 0f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    public bool isSprinting;

    void Update()
    {
        if (InventorySystem.Instance.isOpen == false) // Envanter paneli acik degilse
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // groundCheck objesine dayanarak yere degiyor muyuz kontrol et

            if (isGrounded && velocity.y < 0) // yerdeysek ve dikey dusus hizimiz 0'dan dusukse
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) // herhangi bir shift'e basili tutarsak
            {
                isSprinting = true;
                speed = 17f;
            }

            else
            {
                isSprinting = false;
                speed = 12f;
            }

            if (Input.GetButtonDown("Jump") && isGrounded) // Yerdeyken Jump butonuna basarsak
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}

