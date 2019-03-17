﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementInput;
    public int playerNumber = 0;
    public Color playerColor;
    public Hammer hammer;

    /* --- Input Variables --- */
    private string horizontalAxisName;
    private string verticalAxisName;
    private string jumpButtonName;

    /* ----------- Physics Parameters ------------- */
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float jumpForce = 50f;

    /* --- Jump Variables --- */
    private bool isGrounded;
    private int groundLayer;

    private LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.Find("GameManager(Clone)").GetComponent<LevelManager>();
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        groundLayer = LayerMask.NameToLayer("Ground");
        GetComponent<Renderer>().material.color = playerColor;

        // Assign input names
        horizontalAxisName = "HorizontalPlayer" + playerNumber;
        verticalAxisName = "VerticalPlayer" + playerNumber;
        jumpButtonName = "Jump" + playerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (transform.position.y <= -50f)
        {
            Kill();
        }
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
        movementInput = new Vector3(Input.GetAxis(horizontalAxisName), 0f, Input.GetAxis(verticalAxisName));

        // Jump input
        if (Input.GetButtonDown(jumpButtonName))
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
            isGrounded = false;
        }
    }

    // Kill the player when the hammer falls on them or when they fall off the map.
    public void Kill()
    {
        lm.RoundOver((playerNumber == 0) ? 1 : 0);
        Destroy(gameObject);
    }

    /* ------------------------ COLLISION --------------------------------- */ 
    // Collision check with ground to see if player is grounded.
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            if (!isGrounded) isGrounded = true;
            transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            transform.parent = null;
        }
    }
}
