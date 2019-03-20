using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementInput;
    public int playerNumber = 0;
    public Color playerColor;
    public Hammer hammer;
    private bool canMove = false;

    /* --- Input Variables --- */
    private string horizontalAxisName;
    private string verticalAxisName;
    private string jumpButtonName;

    /* ----------- Physics Parameters ------------- */
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float deathHeight = -25f;

    /* --- Jump Variables --- */
    private bool isGrounded;
    private int groundLayer;

    /* --- Powerup Parameters --- */
    [SerializeField] private float fastHammerDuration = 10f;

    private LevelManager lm;
    private AudioSource[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.Find("GameManager").GetComponent<LevelManager>();
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        groundLayer = LayerMask.NameToLayer("Ground");
        GetComponent<Renderer>().material.color = playerColor;
        sounds = GetComponents<AudioSource>();

        // Assign input names
        horizontalAxisName = "HorizontalPlayer" + playerNumber;
        verticalAxisName = "VerticalPlayer" + playerNumber;
        jumpButtonName = "Jump" + playerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            GetInput();
        else
            movementInput = Vector3.zero;

        if (transform.position.y <= deathHeight)
        {
            if (!sounds[2].isPlaying)
                sounds[2].Play();
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
            sounds[0].pitch = Random.Range(0.9f, 1.1f);
            sounds[0].Play();
        }
    }

    // Kill the player when the hammer falls on them or when they fall off the map.
    public void Kill()
    {
        lm.RoundOver((playerNumber == 0) ? 1 : 0);
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, sounds[2].clip.length);
    }

    public void PowerUp()
    {
        StartCoroutine(FastHammer(fastHammerDuration));
    }

    IEnumerator FastHammer(float duration)
    {
        hammer.SetSpeed(2f, 1.5f, 1.5f, 0.5f, true);
        lm.PowerupEnabled(true);
        sounds[1].Play();

        yield return new WaitForSeconds(duration);

        hammer.SetSpeed(0.5f, 0.75f, 0.75f, 2f, false);
        lm.PowerupEnabled(false);
    }

    /* ------------------------ COLLISION --------------------------------- */ 
    // Collision check with ground to see if player is grounded.
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            if (!collision.gameObject.GetComponent<Tile>().IsFalling)
            {
                if (!isGrounded) isGrounded = true;
                transform.parent = collision.transform;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            transform.parent = null;
        }
    }
    
    public void SetCanMove(bool enabled)
    {
        canMove = enabled;
        hammer.canMove = enabled;
    }
}
