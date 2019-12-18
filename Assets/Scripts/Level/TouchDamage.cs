using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//If we collide with a player
        {
            Vector2 direction = other.gameObject.transform.position - transform.position;
            other.gameObject.GetComponent<PlayerMovement>().takeDamage(direction);
        }

    }
}
