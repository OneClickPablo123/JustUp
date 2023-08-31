using System.Collections;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{

    [Header("Surfaces")]
    public bool isWood;
    public bool isStone;
    public bool isGras;
    public bool isSnow;

    [Header("Properties")]
    public bool canMove;
    public bool canDisappear;
    public bool removePlatform;
    public bool canGrab = true;

    [Header("Movement")]
    public GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed;
    GameObject Player;

    [Header("Disappear")]
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool playerOnPlatform;
    private float timeOnPlatform;
    private bool isDisappearing = false;
    public float disappearTime = 3f;
    public float appearTime = 1f;
    private float fadeSpeed;

    //Position
    internal Vector2 platform;






    private void Awake()
    {

        if (canMove)
        {
            foreach (GameObject waypoint in waypoints)

            {
                waypoint.transform.SetParent(null);
            }
        }
        
        if (canGrab)
        {
          platform = this.transform.position;  
        }


    }

    private void Start()
    {
        Player = GameObject.Find("Player");

        if (canDisappear)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            fadeSpeed = disappearTime;    
        }
       
       
    }

    private void Update()
    {
        PlatformMove();
        Disappear();
        ReactivatePlatform();
        GrabPoints(); 
    }

    public void PlatformMove()
    {
        if (canMove)
        
        {
            
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }

            if (currentWaypointIndex < waypoints.Length)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);

            }
        }
    }

    public void Disappear()
    {
        if (canDisappear)
        {
            if (playerOnPlatform && !isDisappearing)
            {
                timeOnPlatform += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timeOnPlatform / disappearTime);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                if (timeOnPlatform >= disappearTime)
                {
                    isDisappearing = true;
                    gameObject.SetActive(false);
                    timeOnPlatform = 0f;
                    Invoke("ReactivatePlatform", appearTime);
                }
            }
        }   
    }

    private void ReactivatePlatform()
    {
        if (canDisappear)
        {
            isDisappearing = false;
            spriteRenderer.color = originalColor;
            gameObject.SetActive(true);
        }
        
    }

    private void GrabPoints()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canMove)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.transform.SetParent(transform);
                Player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
            }

        }

        if (canDisappear)
        {
            if (collision.CompareTag("Player"))
            {
                playerOnPlatform = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (canMove)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.transform.SetParent(null);
                Player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
            }

        }

        if(canDisappear)
        {
            if (collision.CompareTag("Player"))
            {
                playerOnPlatform = false;
                timeOnPlatform = 0f;
            }
        }
    }
}
