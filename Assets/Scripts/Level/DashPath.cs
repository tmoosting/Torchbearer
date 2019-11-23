using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//If the player enters the collider
        {
            other.gameObject.GetComponent<PlayerMovement>().freeDash = true;//Give the player unlimited dashes
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))//If the player exits our collider
        {
            other.gameObject.GetComponent<PlayerMovement>().freeDash = false;//Remove unlimited dashes from the player
        }
    }
}
