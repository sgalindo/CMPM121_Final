using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private Text winText;
    private Text continueText;
    private Text scoreText;
    private Text playerScoreText0;
    private Text playerScoreText1;
    private Image background;

    // Start is called before the first frame update
    void Awake()
    {
        winText = transform.Find("Win_Text").GetComponent<Text>();
        continueText = transform.Find("Continue_Text").GetComponent<Text>();
        scoreText = transform.Find("Score_Text").GetComponent<Text>();
        playerScoreText0 = scoreText.transform.Find("PlayerScore_Text0").GetComponent<Text>();
        playerScoreText1 = scoreText.transform.Find("PlayerScore_Text1").GetComponent<Text>();
        background = transform.Find("Background").GetComponent<Image>();

        MoveScore(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideText(bool enabled)
    {
        winText.enabled = enabled;
        continueText.enabled = enabled;
        background.enabled = enabled;
    }

    public void HideScore(bool enabled)
    {
        scoreText.enabled = enabled;
        playerScoreText0.enabled = enabled;
        playerScoreText1.enabled = enabled;
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

}
