using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    GameObject gamemanager;
    Gamemanager managerScript;
    SaveGame saveGame;
    SpriteRenderer spriteRenderer;
    public PowerUps jumpPowerUp;
    public PowerUps timePowerUp;
    public PowerUps gravityPowerUp;
    public GameObject canvas;
    RectTransform canvasRectTransform;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    //Item2
    public bool jumpItem;
    //Item3
    public bool timeItem;
    //Item4
    public bool gravityItem;
    

    public void Start()
    {
        gamemanager = GameObject.Find("gamemanager");
        managerScript = gamemanager.GetComponent<Gamemanager>();
        saveGame = gamemanager.GetComponent<SaveGame>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
       

        if (jumpItem == true)
        {
            spriteRenderer.sprite = jumpPowerUp.powerUpSprite;
            itemName.text = jumpPowerUp.powerUpName;
            itemDescription.text = jumpPowerUp.description;      
        }

        if (timeItem == true)
        {
            spriteRenderer.sprite = timePowerUp.powerUpSprite;
            itemName.text = timePowerUp.powerUpName;
            itemDescription.text = timePowerUp.description;
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
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           canvas.SetActive(true);
           UpdateTextCanvas();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }

    private void UpdateTextCanvas()
    {
        float newTextWidth = itemDescription.preferredWidth;
        float newTextHeight = itemDescription.preferredHeight;
        canvasRectTransform.sizeDelta = new Vector2(newTextWidth / 3, newTextHeight * 2);
    }


}
