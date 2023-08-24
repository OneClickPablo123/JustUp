using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //Audio 
    AudioSource audioSource;
    public AudioSource jumpAudioSource;
    public AudioSource jumpVoiceManager;

    //Steps
    public AudioClip[] stepSounds;
    private int currentStepIndex = 0;
    
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
                audioSource.clip = stepSounds[currentStepIndex];
                audioSource.Play();
            }
            currentStepIndex = (currentStepIndex + 1) % stepSounds.Length;

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
}
