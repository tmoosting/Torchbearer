using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Sprite towerLo;
    public Sprite towerHi;
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
}
