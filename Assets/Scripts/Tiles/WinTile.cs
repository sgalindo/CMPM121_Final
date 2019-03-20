using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTile : MonoBehaviour
{
    private bool activated = false;
    public bool Activated { get { return activated; } }

    private GameManager gameManager;
    private LevelManager levelManager;

    private Renderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<Renderer>();
        GameObject gm = GameObject.Find("GameManager");
        levelManager = gm.GetComponent<LevelManager>();
        gameManager = gm.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activated = true;
            rend.material.color = Color.gray;
            if (levelManager.CheckWinTiles())
            {
                levelManager.RoundOver(collision.gameObject.GetComponent<PlayerMovement>().playerNumber);
            }
        }
    }
}
