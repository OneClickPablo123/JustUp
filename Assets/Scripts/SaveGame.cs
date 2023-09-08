using System;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    [SerializeField] internal PlayerStats playerStats;
    [SerializeField] internal MenuStats menuStats;


    public void CreatePlayerStats()
    {
        playerStats = new PlayerStats("", 0f , 0,0);
    }

    public void SavePlayerStats()
    {
        PlayerPrefs.SetString("name", playerStats.name);
        PlayerPrefs.SetFloat("highscore", playerStats.highscore);
        PlayerPrefs.SetFloat("bestTime", playerStats.bestTime);
        PlayerPrefs.SetInt("hasItem", playerStats.hasItem);
        PlayerPrefs.Save();
    }

    public void LoadPlayerStats()
    {
        playerStats = new PlayerStats(PlayerPrefs.GetString("name"), PlayerPrefs.GetFloat("highscore"), PlayerPrefs.GetFloat("bestTime"), PlayerPrefs.GetInt("hasItem"));
    }

    public void CreateMenuStats()
    {
        menuStats = new MenuStats(1);
    }

    public void SaveMenuStats()
    {
        try
        {
            PlayerPrefs.SetInt("touchControls", menuStats.touchControls);
            PlayerPrefs.Save(); // Speichern der PlayerPrefs
            Debug.Log("MenuStats Saved!");
        }
        catch (Exception e)
        {
            Debug.LogError("Fehler beim Speichern der Menüstatistiken: " + e.Message);
        }
    }

    public void LoadMenuStats()
    {
        menuStats = new MenuStats(PlayerPrefs.GetInt("touchControls"));
    }

}
