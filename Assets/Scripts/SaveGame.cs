using UnityEngine;

public class SaveGame : MonoBehaviour
{

    PlayerStats playerStats;
    MenuStats menuStats;


    public void CreatePlayerStats()
    {
        playerStats = new PlayerStats("", 0f , 0,0);
    }

    public void SavePlayerStats()
    {
        PlayerPrefs.SetString("name", playerStats.name);
        PlayerPrefs.SetFloat("highscore", playerStats.highscore);
        PlayerPrefs.SetFloat("bestTime", playerStats.bestTime);
        PlayerPrefs.SetFloat("hasItem", playerStats.hasItem);
    }

    public void LoadPlayerStats()
    {
        playerStats = new PlayerStats(PlayerPrefs.GetString("name"), PlayerPrefs.GetFloat("highscore"), PlayerPrefs.GetFloat("bestTime"), PlayerPrefs.GetInt("hasItem"));
    }

    public void CreateMenuStats()
    {
        menuStats = new MenuStats(0);
    }

    public void SaveMenuStats()
    {
        PlayerPrefs.SetInt("inputSystem", menuStats.inputSystem);
    }

    public void LoadMenuStats()
    {
        menuStats = new MenuStats(PlayerPrefs.GetInt("inputSystem"));
    }

}
