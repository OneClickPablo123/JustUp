using TMPro;
using UnityEngine;


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
    PlayerController playerController;
    GameObject player;
    SaveGame saveGame;
    public TMP_InputField inputfield;
    float playerHighscore;
    string playerName;
    RectTransform canvasRectTransform;
    TouchScreenKeyboard keyboard;




    void Start()
    {
        dialogBox.gameObject.SetActive(false);
        gameManagerO = GameObject.Find("gamemanager");
        gamemanager = gameManagerO.GetComponent<Gamemanager>();
        saveGame = gamemanager.GetComponent<SaveGame>();
        inputBox.SetActive(false);
        canvasRectTransform = dialogBox.GetComponent<RectTransform>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

       if (Application.isMobilePlatform)
        {
            dialogBox.transform.localPosition = new Vector2(0, 500);
        }
           
        
    }

    void Update()
    {
        playerHighscore = saveGame.playerStats.highscore;
        playerName = saveGame.playerStats.name;
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {
            dialogBox.SetActive(true);

            
            if (playerHighscore > 30 && transform.position.y > 30)
            {              
                dialogBoxText.text = "Ooh " + playerName + ",\n We made some progress so far... \n You should check the beds here, \n They got some nice bounce! \n Until we meet again!";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 150 && transform.position.y > 150)
            {
                dialogBoxText.text = "Ahhh Sir " + playerName + ",\nI have decided to face the challenge.\nThe time has finally come.\nI hope to see you at the top!\nMay the best-trained legs win.";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 450 && transform.position.y > 450)
            {
                dialogBoxText.text = "";
                UpdateTextCanvas();
            }
            else if (playerHighscore > 800 && transform.position.y > 800)
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
            if (playerName == "" && transform.position.y < 25 && playerController.IsGrounded())
            {
                if (Application.isMobilePlatform)
                {
                    dialogBoxText.text = "Greetings, fellow traveler. My name is Renaldo, son of my father. \n For generations, we've been trying to escape from this place. \nThe only way is upwards... Please, it would be an honor to\n know your name, sir.";
                    inputBox.SetActive(true);
                    Time.timeScale = 0f;
                    keyboard = TouchScreenKeyboard.Open("PLAYER", TouchScreenKeyboardType.Default);
                    if (keyboard != null && keyboard.active && keyboard.status == TouchScreenKeyboard.Status.Done)
                    {

                        string enteredText = keyboard.text;
                        saveGame.playerStats.name = enteredText;
                        saveGame.SavePlayerStats();
                        inputBox.SetActive(false);
                        Time.timeScale = 1;
                        keyboard = null;
                    }
                }
                else
                {
                    dialogBoxText.text = "Greetings, fellow traveler. My name is Renaldo, son of my father. \n For generations, we've been trying to escape from this place. \nThe only way is upwards... Please, it would be an honor to\n know your name, sir.";
                    inputBox.SetActive(true);
                    Time.timeScale = 0f;
                    inputfield.onEndEdit.AddListener(ConfirmInput);
                }

            }

            if (playerName != "" && transform.position.y < 25)
            {                
                dialogBoxText.text = "Great Mr. " + playerName + "\nA pleasure to meet you. \n My father always used to say: \n 'Son, Never Skip Leg Day!'";
                UpdateTextCanvas();
            }
        }
    }

    private void ConfirmInput(string enteredText)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            saveGame.playerStats.name = enteredText;
            saveGame.SavePlayerStats();
            inputBox.SetActive(false);
            Time.timeScale = 1;
            Debug.Log("Eingegebener Text: " + enteredText);
        }
    }

    private void UpdateTextCanvas()
    {         
        float newTextWidth = dialogBoxText.preferredWidth;
        float newTextHeight = dialogBoxText.preferredHeight;
        canvasRectTransform.sizeDelta = new Vector2(newTextWidth + 150f, newTextHeight + 30f);
        dialogBoxText.transform.localPosition = new Vector2(3, 0);
    }
}
