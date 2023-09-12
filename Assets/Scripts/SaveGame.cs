using System;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    [SerializeField] internal PlayerStats playerStats;
    [SerializeField] internal MenuStats menuStats;


    public void CreatePlayerStats()
    {
        playerStats = new PlayerStats(0, 0f , 0,0);
    }

    public void SavePlayerStats()
    {
        PlayerPrefs.SetInt("firstPlayed", playerStats.firstPlayed);
        PlayerPrefs.SetFloat("highscore", playerStats.highscore);
        PlayerPrefs.SetFloat("bestTime", playerStats.bestTime);
        PlayerPrefs.SetInt("hasItem", playerStats.hasItem);
        PlayerPrefs.Save();
    }

    public void LoadPlayerStats()
    {
        playerStats = new PlayerStats(PlayerPrefs.GetInt("firstPlayed"), PlayerPrefs.GetFloat("highscore"), PlayerPrefs.GetFloat("bestTime"), PlayerPrefs.GetInt("hasItem"));
    }

    public void CreateMenuStats()
    {
        menuStats = new MenuStats(2, 0);
    }

    public void SaveMenuStats()
    {
        try
        {
            PlayerPrefs.SetInt("touchControls", menuStats.touchControls);
            PlayerPrefs.SetInt("easyMode", menuStats.easyMode);
            PlayerPrefs.Save();
            Debug.Log("MenuStats Saved!");
        }
        catch (Exception e)
        {
            Debug.LogError("Fehler beim Speichern der Menüstatistiken: " + e.Message);
        }
    }

    public void LoadMenuStats()
    {
        menuStats = new MenuStats(PlayerPrefs.GetInt("touchControls"), PlayerPrefs.GetInt("easyMode"));
    }

}
