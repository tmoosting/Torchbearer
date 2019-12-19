﻿using System.Collections;
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
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    SceneController.Instance.EndLevel(false, false);
        //if (Input.GetMouseButtonDown(1))
        //    SceneController.Instance.EndLevel(true, false);
        //if (Input.GetMouseButtonDown(2))
        //    SceneController.Instance.EndLevel(true, true);

        if (Input.GetKeyDown(KeyCode.I))
            SceneController.Instance.EndLevel(false, false);
        if (Input.GetKeyDown(KeyCode.O))
            SceneController.Instance.EndLevel(true, false);
        if (Input.GetKeyDown(KeyCode.P))
            SceneController.Instance.EndLevel(true, true);
    }
}
