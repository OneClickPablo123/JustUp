using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu (fileName = "JumpPowerUp", menuName = "PowerUps")]
public class ScriptAbleObjects : ScriptableObject
{
public string name;
public string description;
public Sprite powerUpSprite;
}
