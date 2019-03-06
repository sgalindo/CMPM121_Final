using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    /* --- Private Variables --- */
    private Vector3 movementInput; // Vector that contains all directional input for the hammer machine's movement
    private bool hammerPressed = false; // If the hammer down button is pressed

    private Rigidbody rb;                           // Hammer's Rigidbody component
    [SerializeField] float moveSpeed = 1.0f;        // Speed at which the hammer machine moves (crane-like movement)
    [SerializeField] float hammerDownSpeed = 10f;   // Speed at which the hammer drops
    [SerializeField] float hammerUpSpeed = 2f;      // Speed at which the hammer is raised
    [SerializeField] float hammerCooldown = 2f;     // Time to wait before hammer is raised after hitting the ground

    // References to children of Hammer
    private GameObject hammerHead;
    private GameObject shadow;

    private Vector3 hammerCeiling;     // Top position of hammer
    private float hammerCeilingHeight; // Y value for hammerCeiling

    private Vector3 hammerFloor;     // Bottom position of hammer
    private float hammerFloorHeight; // Y value for hammerFloor  

    private bool hammerDown = false; // If the hammer is on the ground
    private bool hammerDropping = false; // If hammer is currently dropping

    private float timestamp = 0f; // Timestamp variable for cooldown checks

    // Start is called before the first frame update
    void Start()
    {
        hammerHead = transform.Find("HammerHead").gameObject;
        shadow = transform.Find("ShadowCanvas").gameObject;

        // Y value of the highest point for the hammer = hammer's y position at the start
        hammerCeilingHeight = hammerHead.transform.position.y;

        // Y value of the lowest point for the hammer = shadow's y position + (height of hammer / 2)
        hammerFloorHeight = shadow.transform.position.y + (hammerHead.GetComponent<Collider>().bounds.size.y / 2);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set hammerCeiling every frame (to update x and z values)
        hammerCeiling = new Vector3(
            transform.position.x,
            hammerCeilingHeight,
            transform.position.z
        );

        // Set hammerFloor every frame (to update x and z values)
        hammerFloor = new Vector3(
            transform.position.x, 
            hammerFloorHeight, 
            transform.position.z
        );

        // Reset the movementInput at the start of each frame
        movementInput = Vector3.zero;

        // Only check for input if the hammer isn't in its down state
        if (!hammerDropping && !hammerDown)
        {
            /* --- MOVEMENT INPUT --- */
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movementInput += Vector3.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                movementInput += Vector3.right;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                movementInput += Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                movementInput += Vector3.back;
            }

            /* --- HAMMER DOWN ---*/
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                hammerPressed = true;
            }
        }
    }

    // All physics updates are done here
    private void FixedUpdate()
    {
        // If there is input, move at moveSpeed.
        // Else, don't bother moving 0 units and set velocity to zero just in case some weird Rigidbody calculations are made.
        // movementInput is normalized to avoid faster diagonal movement.
        if (movementInput != Vector3.zero)
        {
            rb.MovePosition(transform.position + (movementInput.normalized * moveSpeed * Time.deltaTime));
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        // If hammer down is pressed and the hammer hasn't been dropped, drop it.
        // Else, if it has been dropped and we are past its cooldown, raise it.
        if (hammerPressed && !hammerDown)
        {
            DropHammer();
        }
        else if (hammerDown && Time.time > timestamp)
        {
            RaiseHammer();
        }
    }

    // Moves hammer head towards floor by step increments. Once it reaches the floor, it stops.
    private void DropHammer()
    {
        hammerDropping = true;
        float step = hammerDownSpeed * Time.deltaTime;
        hammerHead.transform.position = Vector3.MoveTowards(hammerHead.transform.position, hammerFloor, step);
        if (hammerHead.transform.position == hammerFloor)
        {
            if (!hammerDown)
            {
                shadow.GetComponent<ParticleSystem>().Play();

                // Enable collisions so things don't go through it when on the ground.
                // Also make it kinematic so that players can't push it around.
                hammerHead.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = true;
            }
            hammerDown = true;
            hammerDropping = false;
            timestamp = Time.time + hammerCooldown;
        }
    }

    // Moves hammer head back towards ceiling by step increments. Once it reaches the ceiling, it stops.
    private void RaiseHammer()
    {
        float step = hammerUpSpeed * Time.deltaTime;
        hammerHead.transform.position = Vector3.MoveTowards(hammerHead.transform.position, hammerCeiling, step);
        if (hammerHead.transform.position == hammerCeiling)
        {
            if (hammerDown)
            {
                // Disable collisions again.
                // Also, make it non-kinematic.
                hammerHead.GetComponent<Collider>().isTrigger = true;
                rb.isKinematic = false;
            }
            hammerDown = false;
            hammerPressed = false;
        }
    }

    // Public accessor for hammerDown.
    public bool GetHammerDown()
    {
        return hammerDown;
    }
}
