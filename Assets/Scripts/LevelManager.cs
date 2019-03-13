using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameManager gm;
    private LevelGenerator levelGen;
    private GameObject[] winTiles;

    public void InitLevel()
    {
        gm = GetComponent<GameManager>();

        levelGen = GetComponent<LevelGenerator>();
        levelGen.GenerateLevel(Vector3.zero, 14, 20, 0.15f, 3, 5);

        winTiles = GameObject.FindGameObjectsWithTag("WinTile");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckWinTiles()
    {
        for (int i = 0; i< winTiles.Length; i++)
        {
            if (winTiles[i].GetComponent<WinTile>().Activated == false)
            {
                return false;
            }
        }
        return true;
    }
}
