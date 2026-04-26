using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float sprintSpeed = 16f;
    public float speed = 10f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    public bool isSprinting;
    Vector3 velocity;
    bool isGrounded;
    // Update is called once per frame
   void Update()
    {
        if (PauseMenu.Instance != null && PauseMenu.Instance.isPaused) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log("isGrounded: " + isGrounded);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        // Only move if we have input
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;
        }
        float currentSpeed = isSprinting ? sprintSpeed : speed;
        if (move.magnitude > 0.1f || !isGrounded)
        {
           controller.Move(move * currentSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(Vector3.zero);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}