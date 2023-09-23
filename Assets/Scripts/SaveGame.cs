using System;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    [SerializeField] internal PlayerStats playerStats;
    [SerializeField] internal MenuStats menuStats;

    public void CreatePlayerStats()
    {
        playerStats = new PlayerStats(0, 0f, 0f, 0);
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
        menuStats = new MenuStats(3, 0, 1, 0.2f, 0.2f, 0);
    }

    public void SaveMenuStats()
    {
        try
        {
            PlayerPrefs.SetInt("touchControls", menuStats.touchControls);
            PlayerPrefs.SetInt("easyMode", menuStats.easyMode);
            PlayerPrefs.SetFloat("masterVolume", menuStats.masterVolume);
            PlayerPrefs.SetFloat("musicVolume", menuStats.musicVolume);
            PlayerPrefs.SetFloat("effectVolume", menuStats.effectVolume);
            PlayerPrefs.SetInt("shadowsEnabled", menuStats.shadowsEnabled);
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
        menuStats = new MenuStats(PlayerPrefs.GetInt("touchControls"), PlayerPrefs.GetInt("easyMode"), PlayerPrefs.GetFloat("masterVolume"), PlayerPrefs.GetFloat("musicVolume"), PlayerPrefs.GetFloat("effectVolume"), PlayerPrefs.GetInt("shadowsEnabled"));
    }

}
