//Used by DashOrbs, a floating sprite that refreshes your dash count
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashOrb : MonoBehaviour
{
    Animator dashAnim; //Animator
    bool waitingRespawn; //Am i waiting to be respawned

    public float respawnTime = 5.0f;//Basic respawn time = 5sec
    private float spawnTime = 0.5f;//Time it takes for spawning animation to finish
    private float timer;//Used to count

    // Start is called before the first frame update
    void Start()
    {
        dashAnim = GetComponent<Animator>();
        waitingRespawn = false;//Not waiting to be respawned, i exist already
        timer = respawnTime + spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingRespawn)//Am I waiting to be respawned
        {
            if (timer < 0)//Has the respawn animation finished
            {
                waitingRespawn = false;
                timer = respawnTime + spawnTime;
            }
            else if (timer < spawnTime)//Can I start respawning
            {
                dashAnim.SetBool("PickedUp", false);//Respawn animation trigger
                timer -= Time.deltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }    
        }
    }
    private void OnTriggerEnter2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (!waitingRespawn)//If we are not currently respawning
        {
            if (other.gameObject.CompareTag("Player"))//If we collide with a player
            {
                dashAnim.SetBool("PickedUp", true);//Start the empty animation
                waitingRespawn = true;
                other.gameObject.GetComponent<PlayerMovement>().dashAvailable = true;//Give the player a dash
            }
        }
    }
}
