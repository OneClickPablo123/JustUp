using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    public GameObject player;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < 0)
        {
            heightText.text = 0 + "m";
        } else
        {
            heightText.text = Mathf.Round(player.transform.position.y).ToString() + "m";
        }

        timer.text = TimeSpan.FromSeconds(Time.time).ToString("hh':'mm':'ss");


    }
}
