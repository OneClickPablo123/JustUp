using System.Security.Claims;
using UnityEngine;

public class CheckPointAnimation : MonoBehaviour
{
    public Animator anim;
    internal string currentState;
    GameObject Player;
    PlayerController playerController;
    public float animTime;
    internal const string spawnPortal = "spawn_portal";
    internal const string idlePortal = "idle_portal";
    internal const string deletePortal = "delete_portal";
 


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (animTime >= 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(spawnPortal))
        {
            ChangeCheckPointState(idlePortal);
        }       
    }

    public void ChangeCheckPointState(string newState)
    {
        // ====================================
        //  CONVERT ANIMATION STATE INTO STRING
        // ====================================
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }
}
