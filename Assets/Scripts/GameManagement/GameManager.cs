using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool newGame = true;
    public int numRounds = 3;

    public static GameManager instance = null;
    private LevelGenerator levelGen;
    private LevelManager levelManager;

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
        levelManager.InitLevel(numRounds, false, newGame);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }
}
