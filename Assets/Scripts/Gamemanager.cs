using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public GameObject player;

    //Height Text
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI highScore;
    float highScoreHeight;
    private bool newHighScore;
    
    //Timer
    public TextMeshProUGUI timer;
    public TextMeshProUGUI bestTime;
    public float bestTimef;

    //Pause Panel
    public GameObject pausePanel;
    public bool isPanelActive;

    //Others
    public bool gameCompleted;

    //Player Stats


    void Start()
    {
        isPanelActive = false;
        pausePanel.SetActive(false);
        highScoreHeight = PlayerPrefs.GetFloat("highscore");
        highScore.text = "Highscore: " + highScoreHeight.ToString() + "m";
        bestTimef = PlayerPrefs.GetFloat("bestTime");
        Debug.Log(bestTimef);
        gameCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        HeightCounter();
        PausePanel();
   
    }

    public void Timer()
    {

        TimeSpan timeSpanActual = TimeSpan.FromSeconds(Time.time);
        string formattedActualTime = FormatTimeSpan(timeSpanActual);
        timer.text = formattedActualTime;      
        TimeSpan timeSpanbest = TimeSpan.FromSeconds(bestTimef);
        string formattedBest = FormatTimeSpan(timeSpanbest);

        if (bestTimef != 0)
        {
            bestTime.text = formattedBest;

        } else
        {
            bestTime.text = "n/a";
        }

        float actualTime = Time.time;
        //Debug.Log(actualTime);

        if (gameCompleted && actualTime < bestTimef || gameCompleted && bestTimef == 0)
        {
            bestTimef = actualTime;
            PlayerPrefs.SetFloat("bestTime", bestTimef);
            Debug.Log("bestTime Set");
        }
    }

    public void HeightCounter()
    {

        if (player.transform.position.y < 0)
        {
            heightText.text = 0 + "m";
        }
        else
        {
            heightText.text = "Jumped: " + Mathf.Round(player.transform.position.y).ToString() + "m";
        }

        float actualHeight;
        actualHeight = Mathf.Round(player.transform.position.y);

        if (actualHeight > highScoreHeight)
        {
            highScoreHeight = actualHeight;
            highScore.text = "Highscore: " + highScoreHeight.ToString() + "m";
            newHighScore = true;
            
        } else if (actualHeight < highScoreHeight && newHighScore) 
        
        {
            PlayerPrefs.SetFloat("highscore", highScoreHeight);
            newHighScore = false;
        }

    }

    public void PausePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPanelActive)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                isPanelActive = false;
            }
            else
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                isPanelActive = true;
            }
        }
    }

   public void Restart()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPanelActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPanelActive = false;
    }

    public string FormatTimeSpan(TimeSpan timeSpan)
    {
        string formattedTime = "";

        if (timeSpan.Hours > 0)
        {
            formattedTime += timeSpan.Hours.ToString("0") + ":";
        }

        if (timeSpan.Minutes > 0 || formattedTime != "")
        {
            formattedTime += timeSpan.Minutes.ToString("0") + ":";
        }

        formattedTime += timeSpan.Seconds.ToString("0");

        if (timeSpan.Milliseconds > 0)
        {
            formattedTime += "." + timeSpan.Milliseconds.ToString("000");
        }

        return formattedTime;
    }
}
