using TMPro;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    GameObject gamemanager;
    Gamemanager managerScript;
    SaveGame saveGame;
    SpriteRenderer spriteRenderer;
    public PowerUps jumpPowerUp;
    public PowerUps runPowerUp;
    public PowerUps gravityPowerUp;
    Canvas canvas;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

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
        saveGame = gamemanager.GetComponent<SaveGame>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;

        if (jumpItem == true)
        {
            spriteRenderer.sprite = jumpPowerUp.powerUpSprite;
            itemName.text = jumpPowerUp.powerUpName;
            itemDescription.text = jumpPowerUp.description;

        }

        if (runItem == true)
        {
            spriteRenderer.sprite = runPowerUp.powerUpSprite;
            itemName.text = runPowerUp.powerUpName;
            itemDescription.text = runPowerUp.description;
        }

        if (gravityItem == true)
        {
            spriteRenderer.sprite = gravityPowerUp.powerUpSprite;
            itemName.text = gravityPowerUp.powerUpName;
            itemDescription.text = gravityPowerUp.description;
        }
    }
    
    public void Update()
    
    {
        if (managerScript.saveGame.playerStats.hasItem == 0 && canPickUp && Input.GetKeyDown(KeyCode.F))
        {
            if (jumpItem == true)
            {
                managerScript.saveGame.playerStats.hasItem = 2;
                saveGame.SavePlayerStats();
                Destroy(this.gameObject);
                Debug.Log("Jump Item Collected Item ID = " + managerScript.saveGame.playerStats.hasItem);
                Debug.Log(PlayerPrefs.GetInt("hasItem"));
            }

            if (runItem == true)
            {
                managerScript.saveGame.playerStats.hasItem = 3;
                saveGame.SavePlayerStats();
                Destroy(this.gameObject);
                Debug.Log("Run Item Collected Item ID = " + managerScript.saveGame.playerStats.hasItem);
            }

            if (gravityItem == true)
            {
                managerScript.saveGame.playerStats.hasItem = 4;
                saveGame.SavePlayerStats();
                Destroy(this.gameObject);
                Debug.Log("Gravity Item Collected, Item ID = " + managerScript.saveGame.playerStats.hasItem);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           canPickUp = true;
           canvas.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickUp = false;
            canvas.enabled = false;
        }
    }


}
