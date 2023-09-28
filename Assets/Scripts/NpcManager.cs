using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    bool canStartDialog = false;
    AudioSource audioSource;
    public AudioClip[] npcVoices;
    private int currentVoiceIndex = 0;
    public TextMeshProUGUI dialogIndicator;

    void Start()
    {
        dialogBox.gameObject.SetActive(false);
        gameManagerO = GameObject.Find("gamemanager");
        gamemanager = gameManagerO.GetComponent<Gamemanager>();
        saveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        canvasRectTransform = dialogBox.GetComponent<RectTransform>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = saveGame.menuStats.masterVolume * saveGame.menuStats.effectVolume;
        dialogIndicator.text = "";

       if (Application.isMobilePlatform)
        {
            dialogBox.transform.localPosition = new Vector2(0, 450);
        }
    
    }

    void Update()
    {
        NpcDialog();
    }

    public void NpcDialog()
    {

        if (Input.GetKeyDown(KeyCode.F) && canStartDialog)
        {
            ShowDialog();
        }

        playerHighscore = saveGame.playerStats.highscore;

        if (transform.position.y < 25 && !isTextDisplayed && dialogBox.activeSelf == true)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Oh God, bless me, a tourist! \n Mr. Cosplay, I need your help... \n A few days ago, I fell into this deep hole, \n and since then, I have never seen daylight again. \n Please guide me to the surface. I need to find my parents.";
            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 30 && transform.position.y > 30 && !isTextDisplayed && dialogBox.activeSelf == true)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Let's go, fellow traveler. \n Here, we will begin our journey. For too long, I was trapped down here. I found this mysterious object that seems like a teleportation machine or something similar. It takes two people to activate it. Let's try it.";
            UpdateTextCanvas();

            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 150 && transform.position.y > 150 && !isTextDisplayed && dialogBox.activeSelf == true)
        {
            PlayNpcVoice();
            dialogBoxText.text = "Ahhh Mr. Landlord,\nI have decided to face the challenge.\nThe time has finally come.\nI hope to see you at the top!\nMay the best-trained legs win.";
            UpdateTextCanvas();
            if (textCoroutine == null)
            {
                textCoroutine = StartCoroutine(ShowText());
            }
        }
        else if (playerHighscore > 450 && transform.position.y > 450 && !isTextDisplayed && dialogBox.activeSelf == true)
        {
            if (Application.isMobilePlatform)
            {
                //Folgt
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
        else if (playerHighscore > 800 && transform.position.y > 800 && !isTextDisplayed && dialogBox.activeSelf == true)
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

            if (Application.isMobilePlatform)
            {
                dialogIndicator.text = "[Touch] - Talk";                
            }
            else
            {
                dialogIndicator.text = "[F] - Talk";
            }
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))

        {           
            dialogBox.SetActive(false);
            canStartDialog = false;
            isTextDisplayed = false;
            dialogIndicator.text = "";
        }
    }

    public void ShowDialog()
    {      
        if (canStartDialog)
        {       
            dialogBox.SetActive(true);
        }
           
    }

}
