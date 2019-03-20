using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        players = new PlayerMovement[2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitLevel(int numRounds, bool winTileMode, bool newGame)
    {
        inGameUI = GameObject.Find("InGame_Canvas").GetComponent<InGameUI>();
        this.numRounds = numRounds;
        restartEnabled = false;

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            players[0] = GameObject.Find("Player0").GetComponent<PlayerMovement>();
            players[1] = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        }

        EnablePlayerMovement(false);

        if (newGame)
        {
            ResetValues();
        }

        inGameUI.SetScore(playerScores[0], playerScores[1]);
        inGameUI.HideRoundOverText(false);
        inGameUI.HideScore(true);

        if (winTileMode)
        {
            levelGen.GenerateLevelWithWinTiles(Vector3.zero, 14, 20, 0.15f, 3, 5);
            winTiles = GameObject.FindGameObjectsWithTag("WinTile");
        }
        else
            levelGen.GenerateLevel(Vector3.zero, 14, 20, 0.15f);

        StartCoroutine(Countdown(1f));
    }

    private IEnumerator Countdown(float time)
    {
        WaitForSeconds interval = new WaitForSeconds(time);
        inGameUI.ShowCountdown(true, "3", time);
        yield return interval;
        inGameUI.ShowCountdown(true, "2", time);
        yield return interval;
        inGameUI.ShowCountdown(true, "1", time);
        yield return interval;
        inGameUI.ShowCountdown(true, "Go!", time);
        yield return interval;
        inGameUI.HideCountdown(false);
        EnablePlayerMovement(true);
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

    private void EnablePlayerMovement(bool enabled)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetCanMove(enabled);
        }
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
        StartCoroutine(RoundOverDelay(winner));
    }

    private IEnumerator RoundOverDelay(int winner)
    {
        yield return new WaitForSeconds(2f);
        if (!restartEnabled)
        {
            players[0].hammer.canMove = false;
            players[1].hammer.canMove = false;
            inGameUI.SetWinText(winner);
            inGameUI.HideRoundOverText(true);
            restartEnabled = true;
            playerScores[winner] += 1;
            inGameUI.SetScore(playerScores[0], playerScores[1]);
            if (playerScores[0] == numRounds || playerScores[1] == numRounds)
                inGameUI.SetContinueText("Restart");
        }
    }

    public void PowerupEnabled(bool enabled)
    {
        inGameUI.ShowPowerupText(enabled);
    }

    public void Continue()
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

    public void Quit()
    {
        gm.LoadScene("MainMenu");
    }
}
