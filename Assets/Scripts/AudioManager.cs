using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //Audio 
    AudioSource audioSource;
    float originalVolume;
    public AudioSource jumpAudioSource;
    public AudioSource jumpVoiceManager;

    GameObject gameManager;
    Gamemanager managerScript;

    //Steps
    public AudioClip[] stoneSounds;
    public AudioClip[] grasSounds;
    public AudioClip[] woodSounds;
    public AudioClip[] snowSounds;
    
    private int currentStepStoneIndex = 0;
    private int currentStepGrasIndex = 0;
    private int currentStepWoodIndex = 0;
    private int currentStepSnowIndex = 0;
    
    //Jump
    public AudioClip[] jumpAudio;
    private int currentJumpIndex = 0;

    //Jump Voices
    public AudioClip[] jumpVoice;
    private int randomVoice;
    private int currentVoiceIndex = 0;

    //Player
    PlayerController playerController;
    private float moveX;
    private bool isGrounded;
    private float rbvelocityX;

    //Pull
    public AudioClip pullSound;
    private bool hasGeneratedRandomValue = false;
    private float randomValue;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        originalVolume = audioSource.volume;
        gameManager = GameObject.Find("gamemanager");
        managerScript = gameManager.GetComponent<Gamemanager>();  
    }

    
    void Update()
    {

        HandleTouchSounds();

        if (playerController.IsGrounded() == true)
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }

        moveX = playerController.moveX;
        rbvelocityX = playerController.rb.velocity.x;

        //WalkSound
        if (moveX != 0 && isGrounded && rbvelocityX > 3.9 || moveX != 0 && isGrounded && rbvelocityX < -3.9)
        {
            if (Input.GetKey(KeyCode.LeftShift) && playerController.IsGrounded())
            {
                audioSource.pitch = 1;
            } else
            {
                audioSource.pitch = 0.89f;
            }

            if (!audioSource.isPlaying)
            {
                if (playerController.isStone)
                {
                    audioSource.clip = stoneSounds[currentStepStoneIndex];
                    audioSource.Play();
                }

                if (playerController.isWood)
                {
                    audioSource.clip = woodSounds[currentStepWoodIndex];
                    audioSource.Play();
                }

                if (playerController.isSnow)
                {
                    audioSource.clip = snowSounds[currentStepSnowIndex];
                    audioSource.Play();
                }

                if (playerController.isGras)
                {
                    audioSource.clip = grasSounds[currentStepGrasIndex];
                    audioSource.Play();
                }
                
            }
            currentStepStoneIndex = (currentStepStoneIndex + 1) % stoneSounds.Length;
            currentStepWoodIndex = (currentStepWoodIndex + 1) % woodSounds.Length;
            currentStepSnowIndex = (currentStepSnowIndex + 1) % snowSounds.Length;
            currentStepGrasIndex = (currentStepGrasIndex + 1) % grasSounds.Length;

        }
        //Jump Sound
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            randomVoice = Random.Range(0, 100);

            if(randomVoice == 42 || randomVoice == 69)
            {
                jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                jumpVoiceManager.Play();

                currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
            }

            jumpAudioSource.clip = jumpAudio[currentJumpIndex];
            jumpAudioSource.Play();
            
            currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;

        }
        //Item Usage Sound
        if (Input.GetKeyDown(KeyCode.LeftAlt) /*&& managerscript.playerStats.hasItem != 0*/)
        {
            
        }

        //PullSound
        float normalizedTime = playerController.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (playerController.isPullUp && normalizedTime >= 0.6f && playerController.anim.GetCurrentAnimatorStateInfo(0).IsName("player_pull"))
        {
            if (!hasGeneratedRandomValue)
            {
                randomValue = Random.Range(0, 10);
                hasGeneratedRandomValue = true;
            }

            if (randomValue >= 7)
            {
                jumpVoiceManager.clip = pullSound;
                jumpVoiceManager.Play();              
            }           
        } else
        {
            hasGeneratedRandomValue = false;
        } 

    }

    public void HandleTouchSounds()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector2 touchPosition = touch.position;

            if (managerScript.saveGame.menuStats.touchControls == 1)
            {
                if (touchPosition.y > Screen.height * 0.15f)
                {
                    if (touch.phase == TouchPhase.Began && isGrounded)
                    {
                        jumpAudioSource.clip = jumpAudio[currentJumpIndex];
                        jumpAudioSource.Play();
                        randomVoice = Random.Range(0, 100);
                    }

                    if (randomVoice == 42 || randomVoice == 69)
                    {
                        jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                        jumpVoiceManager.Play();

                        currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
                    }

                    currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;
                }
            }


            if (managerScript.saveGame.menuStats.touchControls == 2)
            {
                float middleThirdStart = Screen.width * 0.10f;
                float middleThirdEnd = Screen.width * 0.80f;

                if (touchPosition.x >= middleThirdStart && touchPosition.x <= middleThirdEnd)
                {
                    if (touch.phase == TouchPhase.Began && isGrounded)
                    {
                        jumpAudioSource.clip = jumpAudio[currentJumpIndex];
                        jumpAudioSource.Play();
                        randomVoice = Random.Range(0, 100);
                    }

                    if (randomVoice == 42 || randomVoice == 69)
                    {
                        jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                        jumpVoiceManager.Play();

                        currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
                    }

                    currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;
                }
            }

            if (managerScript.saveGame.menuStats.touchControls == 3)
            {
                float leftHalf = Screen.width / 4; // 25% von links
                float rightHalf = Screen.width / 2; // 50% von links

                // Überprüfen Sie, in welchem Bereich die Berührung stattgefunden hat
                if (touchPosition.x < leftHalf)
                {

                }
                else if (touchPosition.x < rightHalf)
                {

                }
                else
                {
                    if (touch.phase == TouchPhase.Began && isGrounded)
                    {
                        jumpAudioSource.clip = jumpAudio[currentJumpIndex];
                        jumpAudioSource.Play();
                        randomVoice = Random.Range(0, 100);
                    }

                    if (randomVoice == 42 || randomVoice == 69)
                    {
                        jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                        jumpVoiceManager.Play();

                        currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
                    }

                    currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;
                }
            }
        }              
    }
}
