using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameUI : MonoBehaviour
{
    LevelManager lm;

    private Text winText;
    private Button continueButton;
    private Button quitButton;
    private Text scoreText;
    private Text powerupText;
    private Text playerScoreText0;
    private Text playerScoreText1;
    private Text countdownText;
    private Image background;

    private IEnumerator flashText;
    private EventSystem es;
    private AudioSource[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        winText = transform.Find("Win_Text").GetComponent<Text>();
        continueButton = transform.Find("Continue_Button").GetComponent<Button>();
        quitButton = transform.Find("Quit_Button").GetComponent<Button>();
        scoreText = transform.Find("Score_Text").GetComponent<Text>();
        powerupText = transform.Find("Powerup_Text").GetComponent<Text>();
        countdownText = transform.Find("Countdown_Text").GetComponent<Text>();
        playerScoreText0 = scoreText.transform.Find("PlayerScore_Text0").GetComponent<Text>();
        playerScoreText1 = scoreText.transform.Find("PlayerScore_Text1").GetComponent<Text>();
        background = transform.Find("Background").GetComponent<Image>();

        lm = GameObject.Find("GameManager(Clone)").GetComponent<LevelManager>();
        continueButton.onClick.AddListener(ContinueOnClick);
        quitButton.onClick.AddListener(QuitOnClick);

        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        sounds = GetComponents<AudioSource>();

        MoveScore(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FlashText(float speed)
    {
        WaitForSeconds interval = new WaitForSeconds(speed);
        while (true)
        {
            powerupText.color = Color.white;
            yield return interval;
            powerupText.color = Color.magenta;
            yield return interval;
        }
    }

    private void ContinueOnClick()
    {
        lm.Continue();
    }

    private void QuitOnClick()
    {
        lm.Quit();
    }

    public void HideRoundOverText(bool enabled)
    {
        winText.enabled = enabled;
        continueButton.gameObject.SetActive(enabled);
        quitButton.gameObject.SetActive(enabled);
        powerupText.enabled = false;
        background.enabled = enabled;
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(es.firstSelectedGameObject);
    }

    public void HideScore(bool enabled)
    {
        scoreText.enabled = enabled;
        playerScoreText0.enabled = enabled;
        playerScoreText1.enabled = enabled;
    }

    public void HideCountdown(bool enabled)
    {
        countdownText.enabled = enabled;
    }

    public void ShowPowerupText(bool enabled)
    {
        powerupText.enabled = enabled;
        if (enabled)
        {
            flashText = FlashText(0.25f);
            StartCoroutine(flashText);
        }
        else if (flashText != null)
        {
            StopCoroutine(flashText);
        }
    }

    public void SetWinText(int winner)
    {
        winText.text = "Player " + (winner + 1).ToString() + " wins!";
        winText.color = (winner == 0) ? Color.red : Color.blue;
        MoveScore(false);
    }

    public void SetScore(int playerScore0, int playerScore1)
    {
        playerScoreText0.text = playerScore0.ToString();
        playerScoreText1.text = playerScore1.ToString();
    }

    public void MoveScore(bool corner)
    {
        if (corner)
        {
            scoreText.rectTransform.anchoredPosition = new Vector3(-109.5f, -1f, 0f);
        }
        else
        {
            scoreText.rectTransform.position = new Vector3((Screen.width + scoreText.rectTransform.sizeDelta.x) / 2, (Screen.height + scoreText.rectTransform.sizeDelta.y) / 2, 0f);
        }
    }

    public void ShowCountdown(bool enabled, string s, float interval)
    {
        if (s == "Go!")
            sounds[1].Play();
        else
            sounds[0].Play();
        countdownText.text = s;
        countdownText.enabled = enabled;
        StartCoroutine(SizeText(1f, interval));
    }

    IEnumerator SizeText(float amount, float interval)
    {
        float time = 0;
        int originalSize = countdownText.fontSize;
        while (time < interval)
        {
            countdownText.fontSize = Mathf.FloorToInt(countdownText.fontSize + amount);
            time += Time.deltaTime;
            yield return 0;
        }
        countdownText.fontSize = originalSize;
    }

    public void SetContinueText(string s)
    {
        continueButton.transform.GetChild(0).GetComponent<Text>().text = s;
    }

    public void PlayButtonSelect() 
    {
        sounds[2].Play();
    }

    public void PlayButtonSubmit() 
    {
        sounds[3].Play();
    }

}
