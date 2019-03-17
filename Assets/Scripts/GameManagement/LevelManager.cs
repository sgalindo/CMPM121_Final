using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameManager gm;
    private LevelGenerator levelGen;
    private GameObject[] winTiles;
    private int numRounds;
    private int currentRound;

    private PlayerMovement[] players;
    private int[] playerScores = { 0, 0 };

    private bool restartEnabled;

    private InGameUI inGameUI;

    private void Awake()
    {
        gm = GetComponent<GameManager>();
        levelGen = GetComponent<LevelGenerator>();
        players = new PlayerMovement[2] { GameObject.Find("Player0").GetComponent<PlayerMovement>(), GameObject.Find("Player1").GetComponent<PlayerMovement>()};
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start") && restartEnabled)
        {
            if (playerScores[0] < numRounds && playerScores[1] < numRounds)
            {
                ResetRound();
            }
            else
            {
                ResetLevel();
            }
        }
    }

    public void InitLevel(int numRounds, bool winTileMode, bool newGame)
    {
        inGameUI = GameObject.Find("InGame_Canvas").GetComponent<InGameUI>();
        this.numRounds = numRounds;
        restartEnabled = false;

        if (newGame)
        {
            ResetValues();
        }

        inGameUI.SetScore(playerScores[0], playerScores[1]);
        inGameUI.HideText(false);
        inGameUI.HideScore(true);

        if (winTileMode)
        {
            levelGen.GenerateLevelWithWinTiles(Vector3.zero, 14, 20, 0.15f, 3, 5);
            winTiles = GameObject.FindGameObjectsWithTag("WinTile");
        }
        else
            levelGen.GenerateLevel(Vector3.zero, 14, 20, 0.15f);
    }

    private void ResetRound()
    {
        currentRound++;
        gm.newGame = false;
        gm.LoadScene("Level1");
    }

    private void ResetLevel()
    {
        gm.newGame = true;
        gm.LoadScene("Level1");
    }

    private void ResetValues()
    {
        restartEnabled = false;
        currentRound = 1;
        playerScores[0] = 0;
        playerScores[1] = 0;
    }

    /* --- Public functions --- */
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

    public void RoundOver(int winner)
    {
        if (!restartEnabled)
        {
            players[0].hammer.canMove = false;
            players[1].hammer.canMove = false;
            inGameUI.SetWinText(winner);
            inGameUI.HideText(true);
            restartEnabled = true;
            playerScores[winner] += 1;
            inGameUI.SetScore(playerScores[0], playerScores[1]);
        }
    }
}
