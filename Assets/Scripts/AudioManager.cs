using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //Audio 
    AudioSource audioSource;
    public AudioSource jumpAudioSource;
    public AudioSource jumpVoiceManager;

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
   

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
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
            if (Input.GetKey(KeyCode.LeftShift))
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
    }

    public void HandleTouchSounds()
    {
        float middleThirdStart = Screen.width * 0.25f;
        float middleThirdEnd = Screen.width * 0.75f;

        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.position.x >= middleThirdStart && touch.position.x <= middleThirdEnd && isGrounded)
            {

                if (touch.phase == TouchPhase.Began && isGrounded)
                {
                    jumpAudioSource.clip = jumpAudio[currentJumpIndex];
                    jumpAudioSource.Play();
                    randomVoice = Random.Range(0, 100);
                    
                }
                currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;

                if (randomVoice == 42 || randomVoice == 69)
                {
                    jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                    jumpVoiceManager.Play();                    
                }
                currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
            }
        }

        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            
            if (touch1.position.x < middleThirdStart && touch2.position.x >= middleThirdStart && touch2.position.x <= middleThirdEnd || touch1.position.x > middleThirdEnd && touch2.position.x >= middleThirdStart && touch2.position.x <= middleThirdEnd)
            {
                if (touch2.phase == TouchPhase.Began && isGrounded)
                {
                    jumpAudioSource.clip = jumpAudio[currentJumpIndex];
                    jumpAudioSource.Play();
                    randomVoice = Random.Range(0, 100);                  
                }
                currentJumpIndex = (currentJumpIndex + 1) % jumpAudio.Length;

                if (randomVoice == 42 || randomVoice == 69)
                {
                    jumpVoiceManager.clip = jumpVoice[currentVoiceIndex];
                    jumpVoiceManager.Play();                  
                }
                currentVoiceIndex = (currentVoiceIndex + 1) % jumpVoice.Length;
            }
        }

    }
}
