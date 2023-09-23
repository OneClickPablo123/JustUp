
[System.Serializable]
public class MenuStats
{
    public int touchControls;
    public int easyMode;
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;
    public int shadowsEnabled;

    public MenuStats(int touchControls, int easyMode, float masterVolume, float musicVolume, float effectVolume, int shadowsEnabled)
    {
        this.touchControls = touchControls;
        this.easyMode = easyMode;
        this.masterVolume = masterVolume;    
        this.musicVolume = musicVolume;
        this.effectVolume = effectVolume;
        this.shadowsEnabled = shadowsEnabled;
    }
}


