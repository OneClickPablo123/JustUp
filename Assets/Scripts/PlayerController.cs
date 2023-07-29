using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
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
    int moveDir = 1;


    // ==========================
    //     Jumps
    // ==========================
    Vector2 jump;
    public float jumpForce;
    public float fallSpeed;
    public float moveXAir;
    public float jumpForceTimer;
    public bool loadJump;
    public float coyoteTime;
    public float coyoteCounter;
    

    // ==========================
    //     Components
    // ==========================
    Rigidbody2D rb;
    private BoxCollider2D coll;

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
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
                
    void Update()
    {
        // ==========================
        //     Input Abfrage X
        // ==========================
        if (jumpForceTimer < 1.05)
        {
            moveX = Input.GetAxisRaw("Horizontal");
        }

        
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

            if (rb.velocity.y < fallSpeed *-1)
            {

                rb.velocity = new Vector2(rb.velocity.x, fallSpeed *-1);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y <= 0)
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

        if (moveX > 0.1f  || moveX < 0.1f )
        {       

            if (moveX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                moveDir = 1;
            } 

            if (moveX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                moveDir = 0;
            }

            if (Input.GetKey(KeyCode.LeftShift) && moveX != 0)
            {
                maxSpeed = 13.5f;

                if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
                {
                    rb.AddForce(new Vector2(moveX * runSpeed, 0f), ForceMode2D.Impulse);
                }
            } 
            else if (moveX != 0 && !Input.GetKey(KeyCode.LeftShift))
            
            {
                maxSpeed = 12f;
                if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
                {
                    rb.AddForce(new Vector2(moveX * speed, 0f), ForceMode2D.Impulse);
                }
            }

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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundMask);
    }
  
    public void Jump()
    {
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
