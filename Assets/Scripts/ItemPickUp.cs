using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    GameObject gamemanager;
    Gamemanager managerScript;
    SaveGame saveGame;
    SpriteRenderer spriteRenderer;

    //Item Types 

    //Item2
    public bool jumpItem;
    //Item3
    public bool runItem;
    //Item4
    public bool gravityItem;

    private bool canPickUp;

    public void Start()
{
    gamemanager = GameObject.Find("gamemanager"); 
    managerScript = gamemanager.GetComponent<Gamemanager>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    
}
    
    public void Update()
    
    {
        if (managerScript.playerStats.hasItem == 0 && canPickUp && Input.GetKeyDown(KeyCode.F))
        {
            if (jumpItem == true)
            {
                managerScript.playerStats.hasItem = 2;
                Destroy(this.gameObject);
                Debug.Log("JumpItem Collected " + managerScript.playerStats.hasItem);
            }

            if (runItem == true)
            {
                managerScript.playerStats.hasItem = 3;
                Destroy(this.gameObject);
                Debug.Log("RunItem Collected " + managerScript.playerStats.hasItem);
            }

            if (gravityItem == true)
            {
                managerScript.playerStats.hasItem = 4;
                Destroy(this.gameObject);
                Debug.Log("Gravity Collected " + managerScript.playerStats.hasItem);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickUp = false;
        }
    }


}