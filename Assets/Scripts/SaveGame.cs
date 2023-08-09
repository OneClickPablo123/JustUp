using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    private PlayerStats playerStats;


    private void CreatePlayerStats()
    {
        playerStats = new PlayerStats("Lefty", 0f , 0);
    }

    public void SaveStats()
    {
        PlayerPrefs.SetString("name", playerStats.name);
        PlayerPrefs.SetFloat("highscore", playerStats.highscore);
        PlayerPrefs.SetFloat("bestTime", playerStats.bestTime);
    }

    public void LoadStats()
    {
        playerStats = new PlayerStats(PlayerPrefs.GetString("name"), PlayerPrefs.GetFloat("highscore"), PlayerPrefs.GetFloat("bestTime"));
    }

}
