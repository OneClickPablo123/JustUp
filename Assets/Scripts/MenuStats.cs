
[System.Serializable]
public class MenuStats
{
    public int touchControls;
    public int easyMode;
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;
    public int shadowsEnabled;
    public int particleEnabled;

    public MenuStats(int touchControls, int easyMode, float masterVolume, float musicVolume, float effectVolume, int shadowsEnabled, int particleEnabled)
    {
        this.touchControls = touchControls;
        this.easyMode = easyMode;
        this.masterVolume = masterVolume;    
        this.musicVolume = musicVolume;
        this.effectVolume = effectVolume;
        this.shadowsEnabled = shadowsEnabled;
        this.particleEnabled = particleEnabled;
    }
}


