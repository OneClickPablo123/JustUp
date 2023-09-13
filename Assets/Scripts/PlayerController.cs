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
    public float jumpForce;
    public float fallSpeed;
    public float coyoteTime;
    public float coyoteCounter;

    [Header("Hang Settings")]
    Vector2 facePosition;
    public bool isHang;
    public bool canHang;
    Vector2 lgrabPos;
    Vector2 rgrabPos;
    public bool isPullUp;
    Vector2 pullPos;

    [Header("Ladder")]
    public bool isLadder;
    public bool isClimbing;
    public float ladderSpeed;
    private float moveY;

    [Header("Touch Settings")]
    public float landTimer;
    public float landTime;

    [Header("JoyStick Settings")]
    GameObject joyStick;
    RectTransform joystickBackground;
    RectTransform joystickHandle;
    public RectTransform touchArea;
    private Vector2 joystickInput = Vector2.zero;
    private Vector2 originalHandlePosition;
    private int inputSystem;

    [Header("CheckPoint Settings")]
    private bool checkPointActive;
    public GameObject checkPointPrefab;
    private GameObject activeCheckPoint;
    private GameObject checkPointButton;



    //Surfaces
    internal bool isWood;
    internal bool isStone;
    internal bool isGras;
    internal bool isSnow;


    // ==========================
    //     Other Variables
    // ==========================

    private float originalGravity;
    internal bool usedItem;
    Camera mainCam;

    // ==========================
    //     Components
    // ==========================
    internal Rigidbody2D rb;
    private BoxCollider2D coll;
    Gamemanager managerscript;
    GameObject gamemanager;
    SpriteRenderer spriteRenderer;
    [SerializeField] PlatformHandler platformHandler;

    // ==========================
    //    LayerMask's
    // ==========================
    [Header("Layermask Settings")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask grabMask;

    // ==========================
    //    Animation Strings
    // ==========================
    private string currentState;
    const string PLAYER_IDLE = ("player_idle");
    const string PLAYER_WALK = ("player_walk");
    const string PLAYER_JUMP = ("player_jump");
    const string PLAYER_FALL = ("player_fall");
    const string PLAYER_HANG = ("player_hang");
    internal const string PLAYER_PULL = ("player_pull");
    internal Animator anim;

    void Start()
    {
        //Get Components / Objects
        rb = GetComponent<Rigidbody2D>();
        coll = GameObject.Find("Groundcast").GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gamemanager = GameObject.Find("gamemanager");
        managerscript = gamemanager.GetComponent<Gamemanager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        joyStick = GameObject.Find("VirtualJoyStick");
        mainCam = Camera.main;
        activeCheckPoint = checkPointPrefab;
        //Start Variables
        originalMaxSpeed = maxSpeed;
        originalMaxSpeedRun = maxSpeedRun;
        originalGravity = rb.gravityScale;
        checkPointButton = GameObject.Find("CheckPointButton");

        //Start Conditions
   

        //Mobile
        if (Application.isMobilePlatform)
        {     
            if (managerscript.saveGame.menuStats.easyMode == 1)
            {
                checkPointButton.SetActive(true);
            }
            else
            {
                checkPointButton.SetActive(false);
            }

            //JoyStickSettings
            if (managerscript.saveGame.menuStats.touchControls == 1)
            {
                joyStick.SetActive(true);
                joystickBackground = joyStick.transform.Find("JoyStickBackground").GetComponent<RectTransform>();
                joystickHandle = joystickBackground.transform.Find("JoyStickHandler").GetComponent<RectTransform>();
                originalHandlePosition = joystickHandle.anchoredPosition;
            } else
            {
                joyStick.SetActive(false);
            }
        } 
        else
        {
            mainCam.orthographicSize = 8f;
            joyStick.SetActive(false);
            checkPointButton.SetActive(false);
        }

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
        } else if (managerscript.saveGame.menuStats.touchControls == 1)
        {
           moveX = GetJoystickInput().x;
           HandleJoyStickInput();
        }
        else if (managerscript.saveGame.menuStats.touchControls == 2)
        {
            HandleTouchInput1();
        }
        else if (managerscript.saveGame.menuStats.touchControls == 3)
        {
            HandleTouchInput2();
        }

        //Set Camera depend on Screen Roatation.

        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            mainCam.orthographicSize = 8.5f;
        }

        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            mainCam.orthographicSize = 5f;
        }




        // ==========================
        //     Methoden Aufrufe
        // ==========================
        Movement();
        Jump();
        LadderMove();
        Animation();
        ItemUsage();
        HandleHang();
        EasyMode();

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
            landTimer -= Time.deltaTime;
            coyoteCounter = coyoteTime;
        }
        else
        {
            landTimer = landTime;
            coyoteCounter -= Time.deltaTime;

            if (!isClimbing && !usedItem && !isHang && !isPullUp)
            {
                rb.gravityScale = 8f;
            }

            //Reduce Velocity.y -0.01, maximum till fallspeed
            if (!isHang && !isPullUp)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y - 0.01f, -fallSpeed));
            }
            //Get some airDrag at velocity.x to reduce acceleration while jumping
            rb.velocity = new Vector2(rb.velocity.x * (1f - airDrag), rb.velocity.y);
        }

        //Get Face Position of Player
        facePosition = new Vector2(spriteRenderer.flipX ? -1f : 1f, 0);
    }

    public void Movement()
    {

        // =================================
        // Abfrage Move +X / -X && Facing 
        // =================================

        if (moveX > 0.1f && !isHang || moveX < 0.1f && !isHang)
        {

            if (moveX > 0)
            {
                //Flip Player Sprite depend on Horizontal Input.
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (moveX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.3f, groundMask);

    }
    public void HandleHang()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {

            if (!IsGrounded() && canHang && !isPullUp)
            {
                SnapToEdge();
            }
       
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            rb.gravityScale = originalGravity;
            isHang = false;
        }

        if (Input.GetKey(KeyCode.Space) && isHang)
        {
            PullUp();
        } 
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isPullUp = false;
        }

    }

    public void SnapToEdge()
    {
        if (lgrabPos != Vector2.zero && facePosition.x == 1 && transform.position.x < lgrabPos.x && !isHang && canHang)
        {
            
            isHang = true;
            this.transform.position = lgrabPos;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
       
        if (rgrabPos != Vector2.zero && facePosition.x == -1 && transform.position.x > rgrabPos.x &&!isHang && canHang)
        {
            isHang = true;
            this.transform.position = rgrabPos;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
       
    }

    public void PullUp()
    {
        isPullUp = true;
        rb.gravityScale = 0;
        float normalizedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //float climbspeed = 50f;

        if (normalizedTime >= 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName(PLAYER_PULL) && (Vector2)transform.position != pullPos)
        {
            this.transform.position = pullPos;
            isHang = false;
        } 

        if ((Vector2)transform.position == pullPos)
        {
            isPullUp = false;
        }
    }
            

    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y <= 0.1 && coyoteCounter > 0 && !isHang && !isPullUp)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            if (!isHang || !isClimbing || !isPullUp)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2.5f);
                //Set Counter to 0 to avoid Double Jump
                coyoteCounter = 0;
            }

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
        if (Application.isMobilePlatform)
        {
            if (managerscript.saveGame.playerStats.hasItem == 2 && !usedItem)
            {
                StartCoroutine(JumpItemPower());
            }

            if (managerscript.saveGame.playerStats.hasItem == 3 && !usedItem)
            {
                StartCoroutine(TimeItemPower());
            }

            if (managerscript.saveGame.playerStats.hasItem == 4 && !usedItem)
            {
                StartCoroutine(GravityItemPower());
            }
        } 
        else
        {
            if (managerscript.saveGame.playerStats.hasItem == 2 && Input.GetKeyDown(KeyCode.LeftAlt) && !usedItem)
            {
                StartCoroutine(JumpItemPower());
            }

            if (managerscript.saveGame.playerStats.hasItem == 3 && Input.GetKeyDown(KeyCode.LeftAlt) && !usedItem)
            {
                StartCoroutine(TimeItemPower());
            }

            if (managerscript.saveGame.playerStats.hasItem == 4 && Input.GetKeyDown(KeyCode.LeftAlt) && !usedItem)
            {
                StartCoroutine(GravityItemPower());
            }

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

            if (isHang && !isPullUp)
            {
                ChangeAnimationState(PLAYER_HANG);
            }

            else if (isPullUp)
            {
                ChangeAnimationState(PLAYER_PULL);
            }
            else if (rb.velocity.y < 0f)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
            else if (rb.velocity.y > 0)
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

    public void EasyMode()
    {
        if (managerscript.saveGame.menuStats.easyMode == 1)
        {
            if (Input.GetKeyDown(KeyCode.R) && IsGrounded())
            {
                if (!checkPointActive)
                {
                    activeCheckPoint = Instantiate(checkPointPrefab, this.transform.position, Quaternion.Euler(0, 0, 0));
                    checkPointActive = true;
                }
                else
                {
                    this.transform.position = activeCheckPoint.transform.position;
                    Destroy(activeCheckPoint);
                    checkPointActive = false;
                }
            }

        }
    }

    public void MobileEasyMode()
    {
        if (managerscript.saveGame.menuStats.easyMode == 1)
        {
            if (IsGrounded())
            {
                if (!checkPointActive)
                {
                    activeCheckPoint = Instantiate(checkPointPrefab, this.transform.position, Quaternion.Euler(0, 0, 0));
                    checkPointActive = true;
                }
                else
                {
                    this.transform.position = activeCheckPoint.transform.position;
                    Destroy(activeCheckPoint);
                    checkPointActive = false;
                }
            }

        }
    }

    private void HandleTouchInput1()
    {

        //Run OR Jump with One Touch Input
        if (Input.touchCount > 0)
        {
            float middleThirdStart = Screen.width * 0.10f;
            float middleThirdEnd = Screen.width * 0.80f;


            foreach (Touch touch in Input.touches)
            {
                // Holen Sie sich die x-Position der Ber�hrung
                float touchX = touch.position.x;
               
                if (touchX < middleThirdStart)
                {
                   
                    if (touch.phase == TouchPhase.Began)
                    {
                        moveX = -1;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        moveX = 0;
                    }

                }
                else if (touchX > middleThirdEnd)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        moveX = 1;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        moveX = 0;
                    }
                }
                else
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (rb.velocity.y < 0.1f && coyoteCounter > 0)
                        {
                            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                        }

                        else if (!IsGrounded() && !isHang && !isPullUp)
                        {
                            SnapToEdge();
                        }
                    }
                    else if (isHang)
                    {
                        if (touch.phase == TouchPhase.Stationary)
                        {
                            PullUp();
                        }
                        else
                        {
                            isPullUp = false;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (rb.velocity.y > 0.1)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                            coyoteCounter = 0;
                        }
                    }
                }
            }
        }
    }

    private void HandleJoyStickInput()
    {

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPosition = touch.position;

                // �berpr�fe, ob die Ber�hrung innerhalb des begrenzten Bereichs liegt
                if (RectTransformUtility.RectangleContainsScreenPoint(touchArea, touchPosition))
                {
                    Vector2 localTouchPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, touchPosition, null, out localTouchPosition);

                    float backgroundWidth = joystickBackground.sizeDelta.x;

                    if (localTouchPosition.x <= backgroundWidth * 0.5f && touch.phase == TouchPhase.Moved)
                    {
                        float sensitivity = 2.5f;
                        float rawX = localTouchPosition.x / (backgroundWidth * 0.5f);
                        joystickInput = new Vector2(Mathf.Clamp(rawX * sensitivity, -1f, 1f), 0f);
                        joystickHandle.anchoredPosition = new Vector2(joystickInput.x * (backgroundWidth * 0.5f), 0f);
                    }                   
                }            
            }
        } else
        {
            joystickHandle.anchoredPosition = originalHandlePosition;
            joystickInput = Vector2.zero;
        }
        

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPosition = touch.position;

                if (touchPosition.y > Screen.height * 0.15f)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (rb.velocity.y < 0.1f && coyoteCounter > 0)
                        {
                            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                        }

                        else if (!IsGrounded() && !isHang && !isPullUp)
                        {
                            SnapToEdge();
                        }
                    }
                    else if (isHang)
                    {
                        if (touch.phase == TouchPhase.Stationary)
                        {
                            PullUp();
                        }
                        else
                        {
                            isPullUp = false;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (rb.velocity.y > 0.1)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                            coyoteCounter = 0;
                        }
                    }
                }
            }
        }       
    }

    private void HandleTouchInput2()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Holen Sie sich die x-Position der Ber�hrung
                float touchX = touch.position.x;

                // Holen Sie sich die Breite des Bildschirms
                float screenWidth = Screen.width;

                // Teilen Sie die linke Seite des Bildschirms in zwei H�lften
                float leftHalf = screenWidth / 4; // 25% von links
                float rightHalf = screenWidth / 2; // 50% von links

                // �berpr�fen Sie, in welchem Bereich die Ber�hrung stattgefunden hat
                if (touchX < leftHalf)
                {
                    
                    if (touch.phase == TouchPhase.Began)
                    {
                        moveX = -1;
                    } else if (touch.phase == TouchPhase.Ended)
                    {
                        moveX = 0;
                    }
                   
                }
                else if (touchX < rightHalf)
                {
                  
                    if (touch.phase == TouchPhase.Began)
                    {
                        moveX = 1;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        moveX = 0;
                    }
                }
                else
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        
                        if (rb.velocity.y < 0.1f && coyoteCounter > 0)
                        {
                            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
                        }

                        else if (!IsGrounded() && !isHang && !isPullUp)
                        {
                            SnapToEdge();
                        }
                    }
                    else if (isHang)
                    {
                        if (touch.phase == TouchPhase.Stationary)
                        {
                            PullUp();
                        }
                        else
                        {
                            isPullUp = false;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (rb.velocity.y > 0.1)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
                            coyoteCounter = 0;
                        }
                    }
                 
                }
            }
            
        }

    }

    public Vector2 GetJoystickInput()
    {
        return new Vector2(Mathf.Round(joystickInput.x), 0);
    }

    IEnumerator JumpItemPower()
    {

        float originalJumpForce = jumpForce;
        jumpForce += 75f;
        usedItem = true;   
        yield return new WaitForSeconds(5f);
        managerscript.saveGame.playerStats.hasItem = 0;
        jumpForce = originalJumpForce;
        usedItem = false;
    }

    IEnumerator GravityItemPower()
    {
        rb.gravityScale = 2;
        usedItem = true;
        yield return new WaitForSeconds(5f);
        managerscript.saveGame.playerStats.hasItem = 0;
        rb.gravityScale = originalGravity;
        usedItem = false;
    }

    IEnumerator TimeItemPower()
    {
        Time.timeScale = 0.7f;
        usedItem = true;
        yield return new WaitForSeconds(5f);
        managerscript.saveGame.playerStats.hasItem = 0;
        Time.timeScale = 1f;
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
        if (collision.gameObject.GetComponent<PlatformHandler>() != null)
        {
            platformHandler = collision.gameObject.GetComponent<PlatformHandler>();
            
            if (platformHandler.canGrab)
            {
                pullPos = platformHandler.pullPos.transform.position;
            }

            if (platformHandler.isWood)
            {
                isWood = true;
            } else
            {
                isWood = false;
            }

            if (platformHandler.isStone)
            {
                isStone = true;
            } else
            {
                isStone = false;
            }

            if (platformHandler.isGras)
            {
                isGras = true;
            } else
            {
                isGras = false;
            }

            if (platformHandler.isSnow)
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

        if (collision.CompareTag("leftGrab"))
        {
            canHang = true;
            lgrabPos = collision.gameObject.transform.position;         
        }
        

        if (collision.CompareTag("rightGrab"))
        {
            canHang = true;
            rgrabPos = collision.gameObject.transform.position;          
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
        if (collision.CompareTag("Platform"))
        {
            
        }
        if (collision.CompareTag("rightGrab"))
        {
            canHang = false;
            rgrabPos = Vector2.zero;
        }
        if (collision.CompareTag("leftGrab"))
        {
            canHang = false;
            lgrabPos = Vector2.zero;
        }
    }

}
