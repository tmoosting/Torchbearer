using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Villager", menuName = "Villager")]

public class Villager : ScriptableObject
      
{
  

    public enum Occupation { Unassigned, Gravedigger, Smith, Juggler, Executioner }
    public enum Power { Unassigned, Dash, Summon, FireBees, Pokerface }

    public string villagerID;
    public Occupation occupation;
    public Power power;

}
