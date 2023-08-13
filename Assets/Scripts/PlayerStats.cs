[System.Serializable]
public class PlayerStats
{
    
    //Player Stats Saves
    public string name;
    public float highscore;
    public float bestTime;
    public int hasItem;


    public PlayerStats (string name, float highscore, float bestTime, int hasItem)
    {
        this.name = name;
        this.highscore = highscore;
        this.bestTime = bestTime;
        this.hasItem = hasItem;
    }


}


