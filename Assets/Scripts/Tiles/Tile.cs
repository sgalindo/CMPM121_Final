using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    private Vector3 lowestPoint;
    private bool isFalling = false;
    public bool IsFalling { get { return isFalling; } }

    private Vector2 levelPosition;

    private Renderer rend;
    private LevelGenerator lg;

    private Color tileColor;
    private Color powerColor;

    private bool power = false;

    // Start is called before the first frame update
    void Start()
    {
        lowestPoint = transform.position + (-transform.up * 20f);
        rend = GetComponent<Renderer>();
        lg = GameObject.Find("GameManager(Clone)").GetComponent<LevelGenerator>();
        tileColor = rend.material.color;
        powerColor = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Fall();
        }
    }

    private void Fall()
    {
        float step = fallSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, lowestPoint, step);
        transform.Rotate((Random.Range(0, 1) == 0) ? Vector3.right : Vector3.forward, rotateSpeed * Time.deltaTime);

        if (transform.position == lowestPoint)
        {
            Destroy(gameObject);
        }
    }

    /* --- Public Functions --- */
    public Vector2 GetPosition()
    {
        return levelPosition;
    }

    public void SetPosition(int x, int y)
    {
        levelPosition = new Vector2((float)x, (float)y);
    }

    public void Kill()
    {
        try
        {
            transform.GetChild(0).parent = null;
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
        }
        lg.RemoveTile(levelPosition);
        isFalling = true;
        rend.material.color = Color.red;
    }

    public void SetPowerup()
    {
        rend.material.color = powerColor;
        power = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && power == true)
        {
            power = false;
            rend.material.color = tileColor;
            collision.gameObject.GetComponent<PlayerMovement>().PowerUp();
        }
    }
}
