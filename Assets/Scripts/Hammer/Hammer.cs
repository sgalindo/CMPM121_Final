using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hammer : MonoBehaviour
{
    // Player references
    public GameObject player;
    private PlayerMovement playerScript;

    private CameraController cameraController; // Camera script reference for screen shake

    /* --- Private Variables --- */
    private Vector3 movementInput; // Vector that contains all directional input for the hammer machine's movement
    private bool hammerPressed = false; // If the hammer down button is pressed
    private int playerNumber;

    private Rigidbody rb;                           // Hammer's Rigidbody component
    [SerializeField] private float moveSpeed = 1.0f;        // Speed at which the hammer machine moves (crane-like movement)
    [SerializeField] private float hammerDownSpeed = 10f;   // Speed at which the hammer drops
    [SerializeField] private float hammerUpSpeed = 2f;      // Speed at which the hammer is raised
    [SerializeField] private float hammerCooldown = 2f;     // Time to wait before hammer is raised after hitting the ground
    [SerializeField] private float shakeDuration = 0.2f; // Duration of screen shake in seconds
    [SerializeField] private float shakeStrength = 0.09f; // Strength of screen shake

    /* --- Input Variables --- */
    private string horizontalAxisName;
    private string verticalAxisName;
    private string smashButtonName;

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
    private float startTime = 0f;

    [HideInInspector] public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        hammerHead = transform.Find("HammerHead").gameObject;
        shadow = transform.Find("ShadowCanvas").gameObject;

        // Y value of the highest point for the hammer = hammer's y position at the start
        hammerCeilingHeight = hammerHead.transform.position.y;

        // Y value of the lowest point for the hammer = shadow's y position + (height of hammer / 2)
        hammerFloorHeight = shadow.transform.position.y + (hammerHead.GetComponent<Collider>().bounds.size.y / 2) - 1f;
        rb = GetComponent<Rigidbody>();

        playerScript = player.GetComponent<PlayerMovement>();
        playerNumber = playerScript.playerNumber;
        GetComponentInChildren<Image>().color = playerScript.playerColor;
        hammerHead.GetComponent<Renderer>().material.color = playerScript.playerColor;

        // Assign input names
        horizontalAxisName = "HorizontalHammer" + playerNumber;
        verticalAxisName = "VerticalHammer" + playerNumber;
        smashButtonName = "Smash" + playerNumber;

        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
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

        if (canMove)
            GetInput();
    }

    private void GetInput()
    {
        // Reset the movementInput at the start of each frame
        movementInput = Vector3.zero;

        // Only check for input if the hammer isn't in its down state
        if (!hammerDropping && !hammerDown)
        {
            /* --- MOVEMENT INPUT --- */
            movementInput = new Vector3(Input.GetAxis(horizontalAxisName), 0f, Input.GetAxis(verticalAxisName));
            
            /* --- HAMMER DOWN ---*/
            if (Input.GetButtonDown(smashButtonName))
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
            if (startTime == 0f) startTime = Time.time;
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
                cameraController.ShakeCamera(shakeDuration, shakeStrength);

                // Enable collisions so things don't go through it when on the ground.
                // Also make it kinematic so that players can't push it around.
                hammerHead.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = true;
            }
            hammerDown = true;
            hammerDropping = false;
            timestamp = Time.time + hammerCooldown;
            startTime = 0f;
        }
    }

    // Moves hammer head back towards ceiling by step increments. Once it reaches the ceiling, it stops.
    private void RaiseHammer()
    {
        //float distCovered = (Time.time - startTime) * hammerUpSpeed * Time.deltaTime;
        //float fracJourney = distCovered / Vector3.Distance(hammerHead.transform.position, hammerCeiling);
        float step = hammerUpSpeed * Time.deltaTime;

        hammerHead.transform.position = Vector3.MoveTowards(hammerHead.transform.position, hammerCeiling, step);
        //hammerHead.transform.position = new Vector3(hammerHead.transform.position.x, EaseInOutQuad(hammerHead.transform.position.y, hammerCeiling.y, fracJourney), hammerHead.transform.position.z);
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
