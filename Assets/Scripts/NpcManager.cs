using System.Collections;
using TMPro;
using UnityEngine;


public class NpcManager : MonoBehaviour
{
   
    public GameObject dialogBox;
    //Not Longer as 130 characters
    [Tooltip("Der String sollte nicht mehr als 130 Zeichen haben.")]
    public TextMeshProUGUI dialogBoxText;
    Gamemanager gamemanager;
    GameObject gameManagerO;
    PlayerController playerController;
    GameObject player;
    SaveGame saveGame;
    float playerHighscore;
    RectTransform canvasRectTransform;
    Coroutine textCoroutine;
    public float letterDelay;
    bool isTextDisplayed = false;
    bool canStartDialog;
    AudioSource audioSource;
    public AudioClip[] npcVoices;
    private int currentVoiceIndex = 0;


    void Start()
    {
        dialogBox.gameObject.SetActive(false);
        gameManagerO = GameObject.Find("gamemanager");
        gamemanager = gameManagerO.GetComponent<Gamemanager>();
        saveGame = gamemanager.GetComponent<SaveGame>();
        canvasRectTransform = dialogBox.GetComponent<RectTransform>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

       if (Application.isMobilePlatform)
        {
            dialogBox.transform.localPosition = new Vector2(0, 500);
        }
           
        
    }

    void Update()
    {
        NpcDialog();
    }

    public void NpcDialog()
    {
        playerHighscore = saveGame.playerStats.highscore;

        if (transform.position.y < 25 && !isTextDisplayed && canStartDialog)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Greetings, fellow traveler. My name is Renaldo, son of my father. \n For generations, we've been trying to escape from this place. \nThe only way is upwards... However, Get outta here or die trying";
            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 30 && transform.position.y > 30 && !isTextDisplayed && canStartDialog)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Ooh Noble Sir,\n We made some progress so far... \n You should check the beds here, \n They got some nice bounce! \n Until we meet again.";
            UpdateTextCanvas();

            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 150 && transform.position.y > 150 && !isTextDisplayed && canStartDialog)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Ahhh Mr. Landlord,\nI have decided to face the challenge.\nThe time has finally come.\nI hope to see you at the top!\nMay the best-trained legs win.";
            UpdateTextCanvas();
            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 450 && transform.position.y > 450 && !isTextDisplayed && canStartDialog)
        {
            if (Application.isMobilePlatform)
            {

            }
            else
            {
                PlayNpcVoice();
                dialogBoxText.text = "Cheers Sir, \n I forgot to mention... You can move with WASD \n Jump with Space and press STRG to Grab an Edge. \n You can also run with Shift. Thank me later!";
                UpdateTextCanvas();
                if (textCoroutine == null)
                {
                    textCoroutine = StartCoroutine(ShowText());
                }
            }

        }
        else if (playerHighscore > 800 && transform.position.y > 800 && !isTextDisplayed && canStartDialog)
        {
            PlayNpcVoice();
            dialogBoxText.text = "";
            UpdateTextCanvas();

            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
    }

    private void UpdateTextCanvas()
    {
        float newTextWidth = dialogBoxText.preferredWidth;
        float newTextHeight = dialogBoxText.preferredHeight;
        canvasRectTransform.sizeDelta = new Vector2(newTextWidth + 150f, newTextHeight + 30f);
        dialogBoxText.transform.localPosition = new Vector2(3, 0);
    }

    public IEnumerator ShowText()
    {
        int totalVisibleCharacters = 0;
        int count = 0;

        while (count < dialogBoxText.text.Length)
        {

            totalVisibleCharacters += 1;
            count += 1;
            dialogBoxText.maxVisibleCharacters = totalVisibleCharacters;
            yield return new WaitForSeconds(letterDelay);
        }
        isTextDisplayed = true;
        textCoroutine = null;
    }

    public void PlayNpcVoice()
    {

        if (!audioSource.isPlaying && canStartDialog)
        {
            audioSource.clip = npcVoices[currentVoiceIndex];
            audioSource.Play();
        }
        currentVoiceIndex = (currentVoiceIndex + 1) % npcVoices.Length;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {
            canStartDialog = true;
            dialogBox.SetActive(true);      
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {
            dialogBox.SetActive(false);
            canStartDialog = false;
            isTextDisplayed = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    
    }


}
