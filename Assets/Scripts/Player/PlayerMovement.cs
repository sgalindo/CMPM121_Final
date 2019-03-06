﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementInput;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float jumpForce = 50f;

    private bool isGrounded;

    private int groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
        {
            Move();
        }
    }

    // Get player's input
    private void GetInput()
    {
        // Movement input (WASD)
        movementInput = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            movementInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementInput += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movementInput += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementInput += Vector3.back;
        }

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // Move player in whatever the input direction is.
    private void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movementInput.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    // Jump only if the player is grounded.
    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Kill the player when the hammer falls on them or when they fall off the map.
    public void Kill()
    {
        Destroy(gameObject);
    }

    /* ------------------------ COLLISION --------------------------------- */ 
    // Collision check with ground to see if player is grounded.
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }
}