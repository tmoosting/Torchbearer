using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger    
{

    public string dangerID;
    public Sprite dangerSprite;
    public string dangerString;
    public int dangerDamage = 1; // 1 or 2

    public Danger (string name, string description, Sprite sprite)
    {
        dangerID = name;
        dangerString = description;
        dangerSprite = sprite;
    }
}
