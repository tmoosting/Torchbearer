using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconTouch : MonoBehaviour
{
    public GameObject Player;
    private void OnTriggerEnter2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//is the colliding object the player?
        {
            Player.GetComponent<PlayerMovement>().endingAnimation += 1;
            Player.GetComponent<PlayerMovement>().PlayerAnimator.SetTrigger("End");
        }
    }

    public void WalkOffScreen()
    {
        if (Player.GetComponent<PlayerMovement>().endingAnimation == 3)
        {
            Player.GetComponent<PlayerMovement>().endingAnimation += 1;
        }
    }
}
