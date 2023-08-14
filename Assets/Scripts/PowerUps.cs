using UnityEngine;


[CreateAssetMenu (fileName = "PowerUpName", menuName = "PowerUps")]
public class PowerUps : ScriptableObject
{
    public string powerUpName;
    public string description;
    public Sprite powerUpSprite;

}
