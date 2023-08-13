using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool hasItem; 
    GameObject gamemanager;
GameManager managerScript;

    public void Start()
{
    gamemanager = GameObject.Find("gamemanager"); 
    managerScript = gamemanager.GetComponent<ManagerScript>();
}
    
    public void Update(){}

        public void OnTriggerEnter2D(Collision collision)

    {
                                if(collision.gameObject.CompareTag("Player")

{
    if(!hasItem && Input.GetKeyDown(KeyCode.F))

    {
    hasItem = true;
    
    }
}

    }
}