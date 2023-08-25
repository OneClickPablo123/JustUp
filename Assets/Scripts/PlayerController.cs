
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public float runSpeed;
    public float maxSpeed;
    public float moveX;
    public float maxSpeedRun;
    private float originalMaxSpeed;
    private float originalMaxSpeedRun;
    public float airDrag;
    

    [Header("Jump Settings")]
    private Vector2 jump;
    public float jumpForce;
    public float fallSpeed;
    public float coyoteTime;
    public float coyoteCounter;

    [Header("Ladder")]
    public bool isLadder;
    public bool isClimbing;
    public float ladderSpeed;
    private float moveY;

    //Surfaces
    internal bool isWood;
    internal bool isStone;
    internal bool isGras;
    internal bool isSnow;
    Surface surface;

    // ==========================
    //     Other Variables
    // ==========================

    private float originalGravity;
    internal bool usedItem;

    // ==========================
    //     Components
    // ==========================
    internal Rigidbody2D rb;
    private BoxCollider2D coll;
    Gamemanager managerscript;
    GameObject gamemanager;

    // ==========================
    //    LayerMask's
    // ==========================
    [Header("Layermask Settings")]
    [SerializeField] LayerMask groundMask;

    // ==========================
    //    Animation Strings
    // ==========================
    private string currentState;
    const string PLAYER_IDLE = ("player_idle");
    const string PLAYER_WALK = ("player_walk");
    const string PLAYER_JUMP = ("player_jump");
    const string PLAYER_FALL = ("player_fall");
    private Animator anim;

    void Start()
    {
        //Get Components / Objects
        rb = GetComponent<Rigidbody2D>();
        coll = GameObject.Find("Groundcast").GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        managerscript = gamemanager.GetComponent<Gamemanager>();
        gamemanager = GameObject.Find("gamemanager");
        
        //Start Variables
        originalMaxSpeed = maxSpeed;
        originalMaxSpeedRun = maxSpeedRun;
        originalGravity = rb.gravityScale;
        
        //Start Conditions
        
    }
   
    void Update()
    {
        // ==========================
        //     Input Abfrage X
        // ==========================

        //Check if Player is Pressing Left or Right (-1 / 1)
       
        if (Input.touchCount == 0)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }

        // ==========================
        //     Methoden Aufrufe
        // ==========================
        Movement();
        Jump();
        LadderMove();
        Animation();
        HandleTouchInputs();
        ItemUsage();


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
                
            }
    }

    public bool IsGrounded()
        {
        //BoxCast if LayerMask == Groundmask
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.5f, groundMask);
        
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

    public void ItemUsage()
    
    {
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
            Time.timeScale = 0.7f;
            if (Input.GetKeyUp(KeyCode.LeftAlt)) 
            {
              Time.timeScale = 1f;
              managerscript.playerStats.hasItem = 0;            
            }
        }

        if (managerscript.playerStats.hasItem == 4 && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCoroutine(GravityItemPower());
            managerscript.playerStats.hasItem = 0;           
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
            if (rb.velocity.y < 0.1f)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
            else
            {
                ChangeAnimationState(PLAYER_JUMP);
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
                    rb.velocity = new Vector2(moveX, moveY);
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

    private void HandleTouchInputs()
    {

        //Run OR Jump with One Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float middleThirdStart = Screen.width * 0.25f;
            float middleThirdEnd = Screen.width * 0.75f;

            if (touch.position.x >= middleThirdStart && touch.position.x <= middleThirdEnd) // Mitte des Bildschirms
            {

                if (touch.phase == TouchPhase.Began && rb.velocity.y < 0.1f && coyoteCounter > 0)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                }
                else if (touch.phase == TouchPhase.Ended && rb.velocity.y > 0.1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                    coyoteCounter = 0;
                }
            }
            // Set MoveX depend on Input
            if (touch.position.x < middleThirdStart) // Left Screen Side
            {
                moveX = -1;
            }
            else if (touch.position.x > middleThirdEnd) // Right Screen Side
            {
                moveX = 1;
            }
            else
            {
                // No Touch Input = 0
                moveX = 0;
            }
        }

        //Run & Jump with two Touch Inputs 
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float middleThirdStart = Screen.width * 0.25f;
            float middleThirdEnd = Screen.width * 0.75f;
           
            //First run, second Jump
            if (touch1.position.x < middleThirdStart && touch2.position.x >= middleThirdStart && touch2.position.x <= middleThirdEnd)
            {
                moveX = -1;

                if (touch2.phase == TouchPhase.Began && rb.velocity.y < 0.1f && coyoteCounter > 0)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                }
                else if (touch2.phase == TouchPhase.Ended && rb.velocity.y > 0.1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                    coyoteCounter = 0;
                }
            } else if (touch1.position.x > middleThirdEnd && touch2.position.x >= middleThirdStart && touch2.position.x <= middleThirdEnd)
            {
                moveX = 1;
                if (touch2.phase == TouchPhase.Began && rb.velocity.y < 0.1f && coyoteCounter > 0)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                }
                else if (touch2.phase == TouchPhase.Ended && rb.velocity.y > 0.1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                    coyoteCounter = 0;
                }
            }

            //First Jump, second Run
            else if (touch1.position.x >= middleThirdStart && touch2.position.x <= middleThirdEnd && touch2.position.x < middleThirdStart)
            {
                moveX = -1;

                if (touch1.phase == TouchPhase.Began && rb.velocity.y < 0.1f && coyoteCounter > 0)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                }
                else if (touch1.phase == TouchPhase.Ended && rb.velocity.y > 0.1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                    coyoteCounter = 0;
                }
            } else if (touch1.position.x >= middleThirdStart && touch1.position.x <= middleThirdEnd && touch2.position.x > middleThirdEnd)
            {
                moveX = 1;
                if (touch2.phase == TouchPhase.Began && rb.velocity.y < 0.1f && coyoteCounter > 0)
                {
                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                }
                else if (touch2.phase == TouchPhase.Ended && rb.velocity.y > 0.1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                    coyoteCounter = 0;
                }
            }

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
                //BounceBed Sound
               
            }                              
        }

        //Checks the Collision Surface to change Audio
        if (collision.gameObject.GetComponent<Surface>() != 0)
        {
           surface = collision.gameObject.GetComponent<Surface>();

           if (surface.isWood)
           {
            isWood = true;
           } else 
           {
            isWood = false;
           }

            if (surface.isStone)
           {
            isStone = true;
           } else 
           {
            isStone = false;
           }

            if (surface.isGras)
           {
            isGras = true;
           } else 
           {
            isGras = false;
           }

            if (surface.isSnow)
           {
            isSnow = true;
           } else 
           {
            isSnow = false;
           }
        }

        //Checks if Player hit Ladder
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if Player leave ladder
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
