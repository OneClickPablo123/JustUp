[System.Serializable]
public class PlayerStats
{
    
    //Player Stats Saves
    public string name;
    public float highscore;
    public float bestTime;

    //NPC Querys
    public int tutorialDialog;

    public PlayerStats (string name, float highscore, float bestTime)
    {
        this.name = name;
        this.highscore = highscore;
        this.bestTime = bestTime;
    }


}


