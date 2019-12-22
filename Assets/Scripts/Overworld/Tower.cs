﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Sprite levelSuccessSprite;
    public Sprite levelFailSprite;
    public Sprite flagSprite;
    public GameObject cleared;
    public GameObject failed;
    [HideInInspector]
    public bool explorable = false;
    [HideInInspector]
    public int towerID;

    private void OnMouseOver()
    {
        //if (explorable == true)
        //gameObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.towerSpriteHi;
    }
    private void OnMouseExit()
    {
        //if (explorable == true)
        //    gameObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.towerSprite;
    }
    private void OnMouseDown()
    {
        if (explorable == true && OverworldController.Instance.GetNarrator().IsNextLevelClickAllowed() == true)
        {
            OverworldController.Instance.ClickTower(this);
        }

    }
    public void SetCompletionSprite()
    {
        if (SceneController.Instance.lastLevelSuccess == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = levelSuccessSprite;
            cleared.SetActive(true);
        }
            
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = levelFailSprite;
            failed.SetActive(true);
        }  
    }
    public void SetFlagSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = flagSprite;
    }
}
