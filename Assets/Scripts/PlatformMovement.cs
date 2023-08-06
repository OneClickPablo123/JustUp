using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed;
    GameObject Player;

    private void Start()

    {
        Player = GameObject.Find("Player");
    }

    private void Awake()
    {
        foreach (GameObject waypoint in waypoints) 
        
        {
            waypoint.transform.SetParent(null);  
        }
    }
    private void FixedUpdate()
    {
       
    }

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Player")
        {            
            collision.gameObject.transform.SetParent(transform);
            Player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            Player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        }

      

    }
}
