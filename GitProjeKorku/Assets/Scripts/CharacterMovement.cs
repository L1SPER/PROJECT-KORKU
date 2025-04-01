using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform orientation;

    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Zeminde miyiz?
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2), groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Zemine yapıştır
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Hareket girdisi
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Yönü al
        Vector3 move = orientation.forward * z + orientation.right * x;

        // Koşma mı yürüyüş mü?
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);
        
        controller.Move(velocity * Time.deltaTime);
    }
}