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
        if (explorable == true)
        gameObject.GetComponent<SpriteRenderer>().sprite = towerHi;
    }
    private void OnMouseExit()
    {
        if (explorable == true)
            gameObject.GetComponent<SpriteRenderer>().sprite = towerLo;
    }
    private void OnMouseDown()
    {
        if (explorable == true)
        {
            UIController.Instance.overworldInterface.ClickTower(this);
        }

    }
}
