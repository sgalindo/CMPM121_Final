using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] float fadeSpeed = 5f;
    private Vector3 lowestPoint;
    private bool isFalling = false;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        lowestPoint = transform.position + (-transform.up * 20f);
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Fall();
        }
    }

    public void Kill()
    {
        isFalling = true;
        rend.material.color = Color.red;
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
}
