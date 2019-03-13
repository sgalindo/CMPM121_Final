using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private LevelGenerator levelGen;
    private LevelManager levelManager;

    private Canvas gameCanvas;
    private int playerScore = 0;
    private int hammerScore = 0;

    private bool restartEnabled;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        levelGen = GetComponent<LevelGenerator>();
        levelManager = GetComponent<LevelManager>();

        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        gameCanvas = GameObject.Find("InGame_Canvas").GetComponent<Canvas>();
        gameCanvas.transform.Find("PlayerScore_Text").GetComponent<Text>().text = playerScore.ToString();
        gameCanvas.transform.Find("HammerScore_Text").GetComponent<Text>().text = hammerScore.ToString();

        SetTextsEnabled(false);
        restartEnabled = false;
        levelManager.InitLevel();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && restartEnabled)
        {
            StartCoroutine(LoadAsyncScene());
        }


    }

    private void SetTextsEnabled(bool enabled)
    {
        Text[] texts = gameCanvas.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].enabled = enabled;
        }
    }

    public void GameOver(bool playerWins)
    {
        if (playerWins)
        {
            gameCanvas.transform.Find("PlayerWin_Text").GetComponent<Text>().enabled = true;
            playerScore++;
        }
        else
        {
            gameCanvas.transform.Find("HammerWin_Text").GetComponent<Text>().enabled = true;
            hammerScore++;
        }
        restartEnabled = true;
        gameCanvas.transform.Find("Restart_Text").GetComponent<Text>().enabled = true;
        gameCanvas.transform.Find("PlayerScore_Text").GetComponent<Text>().text = playerScore.ToString();
        gameCanvas.transform.Find("HammerScore_Text").GetComponent<Text>().text = hammerScore.ToString();
        
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
