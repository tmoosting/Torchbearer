using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//is the colliding object the player?
        {
            other.gameObject.GetComponent<PlayerMovement>().LevelEnd();
            bool timeStatus = other.gameObject.GetComponent<PlayerMovement>().withinTime;
            SceneController.Instance.EndLevel(true, timeStatus);
            //finish level
        }
    }
}
