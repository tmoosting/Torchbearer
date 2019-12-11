using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Villager", menuName = "Villager")]

public class Villager : ScriptableObject
      
{
    //public enum Occupation
    //{
    //    Unassigned, Gravedigger, Smith, Juggler, Executioner, Farmer, Gemcutter, Scribe, Guard, Weaver, Shepherd,
    //    Beekeeper, Armorer, Fisher, Fletcher, Courier, Carpenter, Shaman, Poet, Innkeeper, Herbalist
    //}

    public enum Occupation { Unassigned, Gravedigger, Smith, Juggler, Executioner, Farmer, Gemcutter, Scribe, Guard, Weaver, Shepherd,  
        Beekeeper, Armorer, Fisher, Fletcher, Courier, Carpenter, Shaman, Poet, Innkeeper, Herbalist   }
    public enum Power { Unassigned, Dash, Summon, FireBees, Pokerface }

    public string villagerID;
    public Occupation occupation;
    public Power power;
    public bool isAlive = true;

}
