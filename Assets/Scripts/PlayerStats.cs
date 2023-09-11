[System.Serializable]
public class PlayerStats
{
    
    //Player Stats Saves
    public int firstPlayed;
    public float highscore;
    public float bestTime;
    public int hasItem;


    public PlayerStats (int firstPlayed, float highscore, float bestTime, int hasItem)
    {
        this.firstPlayed = firstPlayed;
        this.highscore = highscore;
        this.bestTime = bestTime;
        this.hasItem = hasItem;
    }


}


