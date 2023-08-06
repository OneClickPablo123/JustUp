using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
   
    public GameObject dialogBox;
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
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
}
