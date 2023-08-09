using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
   
    public GameObject dialogBox;
    //Not Longer as 130 characters
    public string npcText;
    public TextMeshProUGUI dialogBoxText;
    Gamemanager gamemanager;
    GameObject gameManagerO;
    


    void Start()
    {
        dialogBox.gameObject.SetActive(false);
        gameManagerO = GameObject.Find("gamemanager");
        gamemanager = gameManagerO.GetComponent<Gamemanager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            
        {
            dialogBox.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {
            dialogBox.SetActive(false);
        }
    }

    public void ContinueText()
    {


    }

    public void StartDialog()
    {
        
    }
}
