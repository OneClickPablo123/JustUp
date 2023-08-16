
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
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


    [Header("Jump Settings")]
    // ==========================
    //     Jumps
    // ==========================
    private Vector2 jump;
    public float jumpForce;
    public float fallSpeed;
    public float coyoteTime;
    public float coyoteCounter;
    public bool isJumping;

    [Header("Ladder")]
    public bool isLadder;
    public bool isClimbing;
    public float ladderSpeed;
    float moveY;

    // ==========================
    //     Other Variables
    // ==========================

    private float originalGravity;
    public bool usedItem;

    // ==========================
    //     Components
    // ==========================
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    Gamemanager managerscript;
    GameObject gamemanager;

    // ==========================
    //     Masks
    // ==========================
    [Header("Layermask Settings")]
    [SerializeField] LayerMask groundMask;

    // ==========================
    //     Animation
    // ==========================
    private string currentState;
    const string PLAYER_IDLE = ("player_idle");
    const string PLAYER_WALK = ("player_walk");
    const string PLAYER_JUMP = ("player_jump");
    const string PLAYER_FALL = ("player_fall");
    private Animator anim;

    // ==========================
    //     Script Verweise
    // ==========================

    void Start()
    {
    //Get Components
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        originalMaxSpeed = maxSpeed;
        originalMaxSpeedRun = maxSpeedRun;
        originalGravity = rb.gravityScale;
        gamemanager = GameObject.Find("gamemanager");
        managerscript = gamemanager.GetComponent<Gamemanager>();

    }
   
    void Update()
    {
        // ==========================
        //     Input Abfrage X
        // ==========================

        //Check if Player is Pressing Left or Right (-1 / 1)
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");




        // ==========================
        //     Methoden Aufrufe
        // ==========================
        Movement();
        Jump();
        LadderMove();
        Animation();

        // ==========================
        //    Dauerhafte Abfragen
        // ==========================

        if (IsGrounded())
        {
            if (!usedItem)
            {
                rb.gravityScale = originalGravity;
                maxSpeed = originalMaxSpeed;
                maxSpeedRun = originalMaxSpeedRun;
            }
            
            coyoteCounter = coyoteTime;       
            isJumping = false;

        }
        else
        {
            coyoteCounter -= Time.deltaTime;
                    
            if (!isClimbing && !usedItem)
            {
                rb.gravityScale = 8f;
            }

            //Change the Velocity.y -.07, maximum till fallspeed
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y - 0.07f, -fallSpeed));
            //Get some airDrag at velocity.x to reduce acceleration while jumping
            rb.velocity = new Vector2(rb.velocity.x * (1f - airDrag), rb.velocity.y);
        }
    }

    public void Movement()
        {

           
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
                    anim.speed = 1.1f;
                }
                else
                {
                    anim.speed = 1.4f;
                }
            }

        // =================================
        // Item Usage
        // =================================

        if (managerscript.playerStats.hasItem == 2 && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCoroutine(JumpItemPower());
            managerscript.playerStats.hasItem = 0;
        }

        if (managerscript.playerStats.hasItem == 3 && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCoroutine(RunItemPower());
            managerscript.playerStats.hasItem = 0;            
        }

        if (managerscript.playerStats.hasItem == 4 && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCoroutine(GravityItemPower());
            managerscript.playerStats.hasItem = 0;           
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
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
           
            //Set Counter to 0 to avoid Double Jump
            coyoteCounter = 0;
        }

        if (isClimbing && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Animation()
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
        }

        else
        {
            if (rb.velocity.y < 0)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
            else
            {
                ChangeAnimationState(PLAYER_JUMP);
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

    public void LadderMove()
    {
        if (isLadder)
        {

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
            {
                rb.gravityScale = originalGravity;
                isClimbing = false;
                isLadder = false;
                Jump();
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }

            if (isClimbing)
            {
                rb.gravityScale = 0f;

                if (moveY != 0)
                
                {
                    rb.velocity = new Vector2(moveX * 1.5f, moveY * ladderSpeed);
                }

                if (moveY == 0)
                {
                    rb.velocity = new Vector2(moveX, 0);
                }


            }
            else
            {
                rb.gravityScale = originalGravity;
            }


        }
        else

        {
            isClimbing = false;
        }


    }

    IEnumerator JumpItemPower()
    {
        
        float originalJumpForce = jumpForce;
        jumpForce += 75f;
        usedItem = true;
        yield return new WaitForSeconds(5f);
        jumpForce = originalJumpForce;
        usedItem = false;
    }

    IEnumerator RunItemPower()
    {
        float originalRunSpeed = runSpeed;
        float originalMaxSpeed = maxSpeed;
        float originalMaxSpeedRun = maxSpeedRun;
        runSpeed += 20f;
        maxSpeed += 20f;
        maxSpeedRun = 13f;
        usedItem = true;
        yield return new WaitForSeconds(5f);
        runSpeed = originalRunSpeed;
        maxSpeed = originalMaxSpeed;
        maxSpeedRun = originalMaxSpeedRun; 
        usedItem = false;
    }

    IEnumerator GravityItemPower()
    {
        rb.gravityScale = 2;
        usedItem = true;
        yield return new WaitForSeconds(5f);
        rb.gravityScale = originalGravity;
        usedItem = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Check if player collide with jumpbed, add force based on the players y velocity
        if (collision.gameObject.tag == "bouncebed")
        {
            float bounce;
            bounce = collision.gameObject.GetComponent<JumpBed>().bounce;
            if (rb.velocity.y < 0)
            {
                float bounceforce = bounce * rb.velocity.y;
                //Add Force to y axis, MathAbs for positiv Numbers only
                rb.AddForce(new Vector2(rb.velocity.x, Mathf.Abs(bounceforce)), ForceMode2D.Impulse);
            }           
            
                   
        }

        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
