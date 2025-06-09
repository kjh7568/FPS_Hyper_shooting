using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    private CharacterController characterController;
    private Vector3 velocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        ApplyGravity();
    }

    private void MovePlayer()
    {
        Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 move = transform.TransformDirection(inputAxis);

        characterController.Move(move * (moveSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}