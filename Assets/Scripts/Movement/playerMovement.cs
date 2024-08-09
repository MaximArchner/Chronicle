using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class playerMovement : MonoBehaviour
{
    private Animator _animator;
    public CharacterController controller;
    public Animation stillJumpAnimation;

    int isWalkingHash;
    int isSprintingHash;

    public float speed = 0f;
    public float gravity = -60;
    public float jumpHeight = 8f;
    public float jumpDuration = 0.5f;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    public Transform orientation;

    public Vector3 velocity;
    public bool isGrounded;
    public bool isSprinting;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    public bool isMoving;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isSprintingHash = Animator.StringToHash("isSprinting");
    }

    void Update()
    {
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen 
            && !PlayerState.Instance.mainMapIsOpen) // Envanter paneli acik degilse
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // groundCheck objesine dayanarak yere degiyor muyuz kontrol et

            if (isGrounded && velocity.y < 0) // yerdeysek ve dikey dusus hizimiz 0'dan dusukse
            {
                velocity.y = -2f;
                _animator.SetBool("isGrounded", true);
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (x == 0 && z == 0)
            {
                speed = 0f;
                _animator.SetFloat("Speed", 0f);
                _animator.SetBool(isSprintingHash, false);
                _animator.SetBool(isWalkingHash, false);
            }
            else
            {
                Vector3 move = orientation.right * x + orientation.forward * z;
                controller.Move(move * speed * Time.deltaTime);

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) // herhangi bir shift'e basili tutarsak
                {
                    isSprinting = true;
                    speed = 40f;
                    _animator.SetFloat("Speed", 40f);
                    _animator.SetBool(isWalkingHash, false);
                    _animator.SetBool(isSprintingHash, true);
                }

                else
                {
                    isSprinting = false;
                    speed = 30f;
                    _animator.SetFloat("Speed", 30f);
                    _animator.SetBool(isSprintingHash, false);
                    _animator.SetBool(isWalkingHash, true);
                }
            }
            

            if (Input.GetButtonDown("Jump") && isGrounded) // Yerdeyken Jump butonuna basarsak
            {
                _animator.SetTrigger("Jump");
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);


            if (lastPosition != gameObject.transform.position && isGrounded == true)
            {
                isMoving = true;

                SoundManager.Instance.PlaySound(SoundManager.Instance.walkOnGrassSound);
            }
            else
            {

                isMoving = false;

                SoundManager.Instance.walkOnGrassSound.Stop();

            }
            lastPosition = gameObject.transform.position;

        }
    }
    public void Jump()
    {
        _animator.SetBool("isGrounded", false);

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}

