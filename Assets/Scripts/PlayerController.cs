using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // ==========================
    // Movement
    // ==========================
    public float speed;
    public float runSpeed;
    public float maxSpeed;
    private float moveX;
    //private int moveDir = 1;
    public float maxSpeedRun;
    private float originalMaxSpeed;
    private float originalMaxSpeedRun;
    public float airDrag;


    // ==========================
    //     Jumps
    // ==========================
    private Vector2 jump;
    public float jumpForce;
    public float fallSpeed;
    public float coyoteTime;
    public float coyoteCounter;
    public bool isJumping;
    

    // ==========================
    //     Components
    // ==========================
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;

    // ==========================
    //     Masks
    // ==========================
    [SerializeField] LayerMask groundMask;

    // ==========================
    //     Animation
    // ==========================
    private string currentState;
    const string PLAYER_IDLE = ("player_idle");
    const string PLAYER_WALK = ("player_walk");
    const string PLAYER_JUMP = ("player_jump");
    const string PLAYER_RSTART = ("player_runstart");
    private Animator anim;




    void Start()
    {
    //Get Components
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        originalMaxSpeed = maxSpeed;
        originalMaxSpeedRun = maxSpeedRun;  
    }
   
    void Update()
    {
        // ==========================
        //     Input Abfrage X
        // ==========================

        //Check if Player is Pressing Left or Right (-1 / 1)
        moveX = Input.GetAxisRaw("Horizontal");



        // ==========================
        //     Methoden Aufrufe
        // ==========================
        Movement();
        Jump();




        // ==========================
        //    Dauerhafte Abfragen
        // ==========================

        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
            rb.gravityScale = 10;
            isJumping = false;
            maxSpeed = originalMaxSpeed;
            maxSpeedRun = originalMaxSpeedRun;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
            //Limit the Vertical Velocity if the Player is falling down.
            maxSpeed = 11.4f;
            maxSpeedRun = 11.8f;
            rb.gravityScale = 8f;

            //Change the Velocity.y -.07, maximum till fallspeed
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y - 0.07f, -fallSpeed));
            //Get some airDrag at velocity.x to reduce acceleration while jumping
            rb.velocity = new Vector2(rb.velocity.x * (1f - airDrag), rb.velocity.y);
        }
    }


        public void Movement()
        {

            // =================================
            // Animation 
            // =================================

            //If Player not Moving & isGrounded
            if (moveX == 0 && IsGrounded())

            {
                ChangeAnimationState(PLAYER_IDLE);
            }

            if (IsGrounded())
            {
                if (rb.velocity.x > 2.35 && moveX != 0 || rb.velocity.x < -2.35 && moveX != 0)
                {
                    ChangeAnimationState(PLAYER_WALK);
                }

                else if (rb.velocity.x < 2.5 && moveX != 0 || rb.velocity.x > -2.5 && moveX != 0 && anim.name != PLAYER_WALK)
                {
                    ChangeAnimationState(PLAYER_RSTART);
                }

            }

            else
            {
                ChangeAnimationState(PLAYER_JUMP);
            }


            // =================================
            // Abfrage Move +X / -X && Facing 
            // =================================

            if (moveX > 0.1f || moveX < 0.1f)
            {

                if (moveX > 0)
                {
                    //Flip Player Sprite depend on Horizontal Input.
                    GetComponent<SpriteRenderer>().flipX = false;
                    //moveDir = 1;
                }

                if (moveX < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                   // moveDir = 0;
                }
                //Set the maxSpeed at Higher Values if the Player is pressing Shift to Run
                if (Input.GetKey(KeyCode.LeftShift) && moveX != 0)
                {
                    
                
                    //Checks if Velocity is Bigger as maxrunSpeed
                    if (rb.velocity.x < maxSpeedRun && rb.velocity.x > -maxSpeedRun)
                    {
                        //Checks if Velocity is Bigger as MaxSpeed / 2
                        if (rb.velocity.x < maxSpeedRun / 2 || rb.velocity.x > -maxSpeedRun / 2)
                        {
                            //Multiply the RunSpeed * 1.5f 
                            rb.AddForce(new Vector2(moveX * runSpeed * 1.5f * Time.deltaTime, 0f), ForceMode2D.Impulse);
                        }
                        else

                        {
                            //Set the Runspeed to normal Value
                            rb.AddForce(new Vector2(moveX * runSpeed * Time.deltaTime, 0f), ForceMode2D.Impulse);
                        }
                    }

                
                    
                }
                //Set the Velocity to normal Speed if the player is not pushing Shift
                else if (moveX != 0 && !Input.GetKey(KeyCode.LeftShift))

                {
                    
                    //Only Apply force if the players velocity is smaller as maxSpeed
                    if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
                    {
                        rb.AddForce(new Vector2(moveX * speed * Time.deltaTime, 0f), ForceMode2D.Impulse);
                    }
                }
                //Change Animation speed depend on the Players Velocity
                if (rb.velocity.x < 7 || rb.velocity.x > -7f)
                {
                    anim.speed = 0.9f;
                }
                else
                {
                    anim.speed = 1f;
                }
            }
        }

        public bool IsGrounded()
        {
            // ==========================
            //   BOXCAST GROUND ABFRAGE
            // ==========================
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .25f, groundMask);
        }

        public void Jump()
        {
        
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y <= 0.1 && coyoteCounter > 0)
        {
           rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
           
            //Set Counter to 0 to avoid Double Jump
            coyoteCounter = 0;
        }
    }

        void ChangeAnimationState(string newState)
        {
            // ====================================
            //  CONVERT ANIMATION STATE INTO STRING
            // ====================================
            if (currentState == newState) return;
            anim.Play(newState);
            currentState = newState;
        }
}
