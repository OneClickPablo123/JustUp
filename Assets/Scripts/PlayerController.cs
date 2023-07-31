using System.Collections;
using System.Collections.Generic;
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


    // ==========================
    //     Jumps
    // ==========================
    private Vector2 jump;
    public float jumpForce;
    public float fallSpeed;
    public float coyoteTime;
    public float coyoteCounter;
    

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



        // ==========================
        //    Dauerhafte Abfragen
        // ==========================

        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
            rb.gravityScale = 10;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
            rb.gravityScale = 8;

            //Limit the Vertical Velocity if the Player is falling down.
            if (rb.velocity.y < fallSpeed * -1)
            {

                rb.velocity = new Vector2(rb.velocity.x, fallSpeed * -1);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y <= 0.1)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
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
                if (rb.velocity.x > 4.7 && moveX != 0 || rb.velocity.x < -4.7 && moveX != 0)
                {
                    ChangeAnimationState(PLAYER_WALK);
                }

                else if (rb.velocity.x < 4.5 && moveX != 0 || rb.velocity.x > -4.5 && moveX != 0)
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
                    maxSpeed = 13.5f;
                    //Checks if Velocity is Bigger as maxSpeed
                    if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
                    {
                        //Checks if Velocity is Bigger as MaxSpeed / 2
                        if (rb.velocity.x < maxSpeed / 2 || rb.velocity.x > -maxSpeed / 2)
                        {
                            //Multiply the RunSpeed * 1.5f 
                            rb.AddForce(new Vector2(moveX * runSpeed * 1.5f, 0f), ForceMode2D.Impulse);
                        } else

                        {
                            //Set the Runspeed to normal Value
                            rb.AddForce(new Vector2(moveX * runSpeed, 0f), ForceMode2D.Impulse);
                        }
                    }
                }
                //Set the Velocity to normal Speed if the player is not pushing Shift
                else if (moveX != 0 && !Input.GetKey(KeyCode.LeftShift))

                {
                    maxSpeed = 12f;
                    //Only Apply force if the players velocity is smaller as maxSpeed
                    if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
                    {
                        rb.AddForce(new Vector2(moveX * speed, 0f), ForceMode2D.Impulse);
                    }
                }
                //Change Animation speed depend on the Players Velocity
                if (rb.velocity.x > 9.5f || rb.velocity.x < -9.5f)
                {
                    anim.speed = 1.5f;
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
            //Checks if CoyoteCounter is Ready to Avoid double jumps.
            if (coyoteCounter < 0.1) return;
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
