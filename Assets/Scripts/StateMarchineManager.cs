using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StateMarchineManager : MonoBehaviour
{

    public GameObject MainMenuPanel, OptionPanel, PauseButton;
    public Text TextTime;
    float playTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
       
        Time.timeScale = 0f;
        PauseButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        TextTime.text = "Time: " + (int)playTime;
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        MainMenuPanel.SetActive(true);
        GameObject.Find("PlayButton").GetComponentInChildren<Text>().text = "Resume";

    }

    public void Onplay()
    {

        MainMenuPanel.SetActive(false);
        Time.timeScale = 1;
        PauseButton.SetActive(true);
    }

    public void onOption()
    {
        MainMenuPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void onExit()
    {

    }

    public void onOptioBack()
    {
        MainMenuPanel.SetActive(transform);
        OptionPanel.SetActive(false);
    }
}
