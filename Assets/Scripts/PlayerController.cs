using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // ==========================
    // Movement
    // ==========================
    public float speed;
    public float maxSpeed;
    private float moveX;
    int moveDir = 1;


    // ==========================
    //     Jumps
    // ==========================
    Vector2 jump;
    public float jumpForce;
    public float jumpForceTimer;
    public bool loadJump;
    float jumpAnimationTimer;

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
        Jump();



        // ==========================
        //    Dauerhafte Abfragen
        // ==========================
        
        //Legt den Beginn der Walk Animation fest.
        if (!IsGrounded())
        {
            jumpAnimationTimer += Time.deltaTime;
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
        
        else if (moveX != 0 && IsGrounded() && !loadJump)
        {   
           if (jumpAnimationTimer == 0)
            {
                ChangeAnimationState(PLAYER_WALK);
            }     
        }
        
        else if (!IsGrounded() && moveX == 0 || !IsGrounded() && moveX != 0) 
        
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

            if (rb.velocity.x < maxSpeed && rb.velocity.x > -maxSpeed)
            {
                rb.AddForce(new Vector2(moveX * speed, 0f), ForceMode2D.Impulse);
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
        jump = Vector2.up;

        if (Input.GetKey(KeyCode.Space) && IsGrounded() && moveX == 0)
        {
            jumpAnimationTimer += Time.deltaTime;
            jumpForceTimer += Time.deltaTime;
            loadJump = true;

            if (jumpForceTimer < 1)
            {
                jumpForceTimer = 1;
            }

            if (jumpForceTimer > 1.6)
            {
                jumpForceTimer = 1.6f;
            }
        }
        else
        {
            loadJump = false;

            if (moveX != 0 && IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0f, jump.y * jumpForce), ForceMode2D.Impulse);
            } else
            {
                rb.AddForce(new Vector2(0f, jump.y * jumpForce * jumpForceTimer), ForceMode2D.Impulse);
                jumpForceTimer = 0f;
            }
            
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
