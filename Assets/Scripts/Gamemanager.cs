using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public GameObject player;

    //Height Text
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI highScore;
    public float highScoreHeight;
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
    SaveGame saveGame;
    public PlayerStats playerStats;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    //Sounds
    public AudioClip spawnMusic;
    public AudioClip level2Sound;
    public AudioClip level3Sound;
        


    void Start()
    {
        //Pause Panel
        isPanelActive = false;
        pausePanel.SetActive(false);
        
        //High Score
        highScoreHeight = playerStats.highscore;
        highScore.text = "Highscore: " + highScoreHeight.ToString() + "m";
        //Best Time
        bestTimef = playerStats.bestTime;
        gameCompleted = false;

        //Load SaveGame 
        saveGame = GetComponent<SaveGame>();
        saveGame.LoadStats();

        //Get Audio Source
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        HeightCounter();
        PausePanel();
        SoundHandler();
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
        

        if (gameCompleted && actualTime < bestTimef || gameCompleted && bestTimef == 0)
        {
            bestTimef = actualTime;
            saveGame.SaveStats();
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
            playerStats.highscore = highScoreHeight;
            saveGame.SaveStats();
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
        saveGame.SaveStats();
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

    public void SoundHandler()
    {
        if (player.transform.position.y < 30)
        {
            audioSource1.clip = spawnMusic;
            
            if (!audioSource1.isPlaying && audioSource2.volume == 0)
            {
                audioSource1.Play();    
            }
            
            if (audioSource1.volume <= 0.14f)
            {
                audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0.14f, 0.15f * Time.deltaTime);
            }

        } else
        {
            if (audioSource1.clip == spawnMusic)
            {
                audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0, 0.2f * Time.deltaTime);
            }
        }

        
        if (player.transform.position.y > 30 && player.transform.position.y < 50)
        {                     
            audioSource2.clip = level2Sound;
            
            if (!audioSource2.isPlaying && audioSource1.volume == 0)
            {
                audioSource2.Play();
            }

            if (audioSource2.volume < 0.14f)
            {
                audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, 0.14f, 0.15f * Time.deltaTime);
            }
        } 
        
        else
        {
            if (audioSource2.clip == level2Sound)
            {
                audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, 0, 0.2f * Time.deltaTime);
                Debug.Log("LOWEER");
            }
           
        }

        if (player.transform.position.y > 50)
        {
            audioSource1.clip = level3Sound;

            if (!audioSource1.isPlaying && audioSource2.volume == 0)
            {
                audioSource1.Play();
            }

            if (audioSource1.volume < 0.14f)
            {
                audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0.14f, 0.15f * Time.deltaTime);
            }
        }

        else
        {
            if (audioSource1.clip == level3Sound)
            {
                audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0, 0.2f * Time.deltaTime);
            }
        }

    }
}
