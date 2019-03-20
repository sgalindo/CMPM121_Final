using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    private Button playButton;
    private EventSystem es;
    private GameManager gm;

    private AudioSource[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        playButton = transform.Find("Play_Button").GetComponent<Button>();
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        playButton.onClick.AddListener(PlayOnClick);

        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayOnClick()
    {
        gm.LoadScene("Level1");
    }

    public void PlayButtonSelect()
    {
        sounds[0].Play();
    }
}
