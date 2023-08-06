using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public GameObject player;

    //Height Text
    public TextMeshProUGUI heightText;
    //Timer
    public TextMeshProUGUI timer;

    //Pause Panel
    public GameObject pausePanel;
    public bool isPanelActive;

    // Start is called before the first frame update
    void Start()
    {
        isPanelActive = false;
        pausePanel.SetActive(false);
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
        timer.text = TimeSpan.FromSeconds(Time.time).ToString("hh':'mm':'ss");
    }

    public void HeightCounter()
    {

        if (player.transform.position.y < 0)
        {
            heightText.text = 0 + "m";
        }
        else
        {
            heightText.text = Mathf.Round(player.transform.position.y).ToString() + "m";
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
}
