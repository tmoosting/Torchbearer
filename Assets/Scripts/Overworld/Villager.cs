using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Villager", menuName = "Villager")]

public class Villager : ScriptableObject
      
{
  

    public enum Occupation { Gravedigger, Smith, Juggler, Executioner }

    public string villagerID;
    public Occupation occupation;

}
