[System.Serializable]

public class PlayerStats
{
    
    //Player Stats Saves
    public int firstPlayed;
    public float highscore;
    public float bestTime;
    public int hasItem;
    public float spawnPosX;
    public float spawnPosY;
    public float actualTime;


    public PlayerStats (int firstPlayed, float highscore, float bestTime, int hasItem, float spawnPosX, float spawnPosY, float actualTime)
    {
        this.firstPlayed = firstPlayed;
        this.highscore = highscore;
        this.bestTime = bestTime;
        this.hasItem = hasItem;
        this.spawnPosX = spawnPosX;
        this.spawnPosY = spawnPosY;
        this.actualTime = actualTime;
    }


}


