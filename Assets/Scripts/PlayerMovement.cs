using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementInput;

    [SerializeField] private string VerticalAxisName = "VerticalPlayer";
    [SerializeField] private string HorizontalAxisName = "HorizontalPlayer";

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float friction = 1f;
    [SerializeField] private float gravity = 9.8f;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
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

        if (isGrounded)
        {
            ApplyFriction();
        }
        else
        {
            ApplyGravity();
        }
    }

    // Get the input from WASD
    private void GetInput()
    {
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
    }

    // Move player in whatever the input direction is.
    private void Move()
    {
        rb.AddForce(movementInput.normalized * moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    // Apply friction to player (backward acceleration).
    private void ApplyFriction()
    {
        rb.AddForce(-rb.velocity.normalized * friction * Time.deltaTime, ForceMode.Acceleration);
    }

    // Apply gravity to player (downward acceleration).
    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Acceleration);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
