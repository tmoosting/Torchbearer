﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float movementSpeed;

    [HideInInspector]
    public bool isMoving = false;

    public IEnumerator MoveHeroToTower(Tower tower)
    {
        isMoving = true;
        SoundController.Instance.PlayHeroMoves();
        Vector3 towerPos = new Vector3(tower.gameObject.transform.localPosition.x, tower.gameObject.transform.localPosition.y - 2.2f, tower.gameObject.transform.localPosition.z); 

        int count = 0;
        
        while (gameObject.transform.localPosition != towerPos)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, towerPos, movementSpeed);
            if (count > 100000)
                break;
            yield return null; 
        }
        isMoving = false;
        GameController.Instance.TransitionToLevel();
    }
}
