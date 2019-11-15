using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollection : MonoBehaviour
{

    public static SpriteCollection Instance;

    private void Awake()
    {
        Instance = this;
    }
    [Header("Overworld")]
    public Sprite towerSprite;
    public Sprite towerSpriteHi;
    public Sprite dangerMarkerSprite;
    public Sprite heroSprite;
    public Sprite groupSprite;
    public Sprite monsterSprite;

}
