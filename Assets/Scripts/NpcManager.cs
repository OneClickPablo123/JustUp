using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
   
    public GameObject dialogBox;
    public TextMeshProUGUI nameInput;
    public GameObject inputBox;
    //Not Longer as 130 characters
    [Tooltip("Der String sollte nicht mehr als 130 Zeichen haben.")]
    public string npcText;   
    public TextMeshProUGUI dialogBoxText;
    Gamemanager gamemanager;
    GameObject gameManagerO;
    SaveGame saveGame;
    public TMP_InputField inputfield;
    float playerHighscore;
    string playerName;
    public Canvas canvas;
    RectTransform canvasRectTransform;
    RectTransform textRectTransform;




    void Start()
    {
        dialogBox.gameObject.SetActive(false);
        gameManagerO = GameObject.Find("gamemanager");
        gamemanager = gameManagerO.GetComponent<Gamemanager>();
        saveGame = gamemanager.GetComponent<SaveGame>();
        inputBox.SetActive(false);
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        textRectTransform = dialogBoxText.GetComponent<RectTransform>();
    }

    void Update()
    {
        playerHighscore = gamemanager.playerStats.highscore;
        playerName = gamemanager.playerStats.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            
        {
            dialogBox.SetActive(true);

            if (playerName == "")
            {
                dialogBoxText.text = "Greetings, fellow traveler. \n My name is Renaldo, son of my father. \n For generations, we've been trying to escape from this place. \nThe only way is upwards...\n Please, it would be an honor \n to know your name, sir.";
                inputBox.SetActive(true);
                inputfield.onEndEdit.AddListener(ConfirmInput);
            }
            else if (playerHighscore > 25)
            {
                dialogBoxText.text = "Ahhh Sir " + playerName + ",\nI have decided to face the challenge.\nThe time has finally come.\nI hope to see you at the top!\nMay the best-trained legs win.";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 150)
            {
                dialogBoxText.text = "";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 450)
            {
                dialogBoxText.text = "";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 800)
            {
                dialogBoxText.text = "";
                UpdateTextCanvas();
            }

            
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {
            dialogBox.SetActive(false);
            inputBox.SetActive(false) ;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (playerName != "" && playerHighscore < 25)
            {                
                dialogBoxText.text = "Great Mr. " + playerName + "\nA pleasure to meet you. \n My father used to say: \n 'Son, Never Skip Leg Day!'";
                UpdateTextCanvas();
            }
        }
    }

    private void ConfirmInput(string enteredText)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gamemanager.playerStats.name = enteredText;
            saveGame.SaveStats();
            inputBox.SetActive(false);
            Debug.Log("Eingegebener Text: " + enteredText);
        }
    }

    private void UpdateTextCanvas()
    {         
        float newTextWidth = dialogBoxText.preferredWidth;
        float newTextHeight = dialogBoxText.preferredHeight;
        canvasRectTransform.sizeDelta = new Vector2(newTextWidth + 0.5f, newTextHeight + 0.4f);
    }
}
