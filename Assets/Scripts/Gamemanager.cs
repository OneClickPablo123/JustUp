using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{

    

    [Header("HEIGHT")]
    //Height Text
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI highScore;
    public float highScoreHeight;
    private bool newHighScore;

    [Header("TIMER")]
    //Timer
    public TextMeshProUGUI timer;
    public TextMeshProUGUI bestTime;
    public float bestTimef;

    [Header("PAUSE PANEL")]
    //Pause Panel
    public GameObject pausePanel;
    public GameObject pause;
    public bool isPanelActive;

    [Header("OTHERS")]
    //Others
    public bool gameCompleted;
    internal SaveGame saveGame;
    private GameObject player;
    private PlayerController playerController;

    [Header("SOUNDS")]
    //Sounds
    public AudioSource audioSource;
    public AudioClip spawnMusic;
    public AudioClip level2Sound;
    public AudioClip level3Sound;
    float musicVolume;
    public float spawnMusicThreshold = 30f;
    public float level2Threshold = 50f;
    public float level3Threshold = 80f;
    public float volumeChangeSpeed = 0.15f;

    //MenuStats
    [SerializeField] internal int touchControls;

    [Header ("ITEMS")]
    private GameObject itemButton;
    private GameObject placeHolder;
    internal Image itemImage;
    public Sprite jumpItem;
    public Sprite timeItem;
    public Sprite gravityItem;
    public Sprite pickUpItem;


    private void Awake()
    {
        //Load SaveGame     
        saveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        saveGame.LoadMenuStats();
        saveGame.LoadPlayerStats();
    }
    void Start()
    {
        //Player
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        //Pause Panel
        isPanelActive = false;
        pausePanel.SetActive(false);

        //High Score
        highScoreHeight = saveGame.playerStats.highscore;
        highScore.text = "Highscore: " + highScoreHeight.ToString() + "m";
        
        //Best Time
        bestTimef = saveGame.playerStats.bestTime;
        gameCompleted = false;

        //Audio Handler

        //ItemButton
        itemButton = GameObject.Find("ItemButton");
        itemButton.SetActive(true);
        placeHolder = itemButton.transform.Find("PlaceholderImage").gameObject;
        itemImage = placeHolder.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        HeightCounter();
        PausePanel();
        AudioHandler();
        ItemButton();
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
            saveGame.playerStats.bestTime = bestTimef;
            saveGame.SavePlayerStats();
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
            saveGame.playerStats.highscore = highScoreHeight;
            saveGame.SavePlayerStats();
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

    public void TouchPause()
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

   public void Restart()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPanelActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }

    public void ExitGame()
    {
        saveGame.SaveMenuStats();
        saveGame.SavePlayerStats();
        Application.Quit();       
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPanelActive = false;
    }

    public void BackToMenu()
    {
        saveGame.SaveMenuStats();
        saveGame.SavePlayerStats();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ItemButton()
    {
        Color color = Color.white;

        if (saveGame.playerStats.hasItem == 0)
        {
            if (playerController.canPickUp)
            {
                itemImage.sprite = pickUpItem;
                color.a = 1f;
                itemImage.color = color;
            }
            else
            {
                itemImage.sprite = null;
                color.a = 0f;
                itemImage.color = color;
            }

        }
        else if (saveGame.playerStats.hasItem == 2)
        {
            itemImage.sprite = jumpItem;
            color.a = 1f;
            itemImage.color = color;
        }
        else if (saveGame.playerStats.hasItem == 3)
        {
            itemImage.sprite = timeItem;
            color.a = 1f;
            itemImage.color = color;
        } 
        else if (saveGame.playerStats.hasItem == 4)
        {
            itemImage.sprite = gravityItem;
            color.a = 1f;
            itemImage.color = color;
        }

        
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

    public void HandleClip (AudioClip clip) 
    
    {
     if (audioSource.clip != clip) 
     {         
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0, volumeChangeSpeed * Time.deltaTime);

       if (audioSource.volume == 0)
       {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
       }
     }
     else if (audioSource.volume < musicVolume)
    {
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, musicVolume, volumeChangeSpeed * Time.deltaTime);
    }
    else if (audioSource.volume > 0)
    {
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0, volumeChangeSpeed * Time.deltaTime);
    }
    }

    public void AudioHandler() 
    {
        musicVolume = saveGame.menuStats.masterVolume * saveGame.menuStats.musicVolume;

        float playerY = player.transform.position.y;

        if (playerY < spawnMusicThreshold)
        {
            HandleClip(spawnMusic);
        }
        else if (playerY < level2Threshold && playerY > spawnMusicThreshold)
        {
            HandleClip(level2Sound);
        }
        else if (playerY < level3Threshold && playerY > level2Threshold)
        {
            HandleClip(level3Sound);
        }
    }
}
