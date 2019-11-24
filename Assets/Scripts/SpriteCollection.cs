﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollection : MonoBehaviour
{

    public static SpriteCollection Instance;
    public List<Sprite> dangerSpriteList = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        dangerSpriteList.Add(danger1Sprite);
        dangerSpriteList.Add(danger2Sprite);
        dangerSpriteList.Add(danger3Sprite);
        dangerSpriteList.Add(danger4Sprite);
        dangerSpriteList.Add(danger5Sprite);
        dangerSpriteList.Add(danger6Sprite);
        dangerSpriteList.Add(danger7Sprite);
        dangerSpriteList.Add(danger8Sprite);
        dangerSpriteList.Add(danger9Sprite);
        dangerSpriteList.Add(danger10Sprite);
    }
    [Header("Narrator - Introduction")]
    public Sprite screen1Sprite;
    public Sprite screen2Sprite1;
    public Sprite screen2Sprite2;
    public Sprite screen3Sprite1;
    public Sprite screen3Sprite2;
    public Sprite screen4Sprite;
    [Header("Narrator - Events")]
    public Sprite heroStandsUpSprite;
    public Sprite successfulLevelSprite;
    public Sprite dangerDodgedSprite;
    public Sprite danger1Sprite;
    public Sprite danger2Sprite;
    public Sprite danger3Sprite;
    public Sprite danger4Sprite;
    public Sprite danger5Sprite;
    public Sprite danger6Sprite;
    public Sprite danger7Sprite;
    public Sprite danger8Sprite;
    public Sprite danger9Sprite;
    public Sprite danger10Sprite;

    [Header("Overworld")]
    public Sprite towerSprite;
    public Sprite towerSpriteHi;
    public Sprite dangerMarkerSprite;
    public Sprite heroSprite;
    public Sprite groupSprite;
    public Sprite monsterSprite;

}
