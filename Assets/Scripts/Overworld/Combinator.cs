using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combinator", menuName = "Combinator")]
public class Combinator : ScriptableObject
{
    List<Villager.Occupation> allOccupations = new List<Villager.Occupation>();

    public string combinationString;
    public Villager.Occupation occupation1 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation2 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation3 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation4 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation5 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation6 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation7 = Villager.Occupation.Unassigned;
    public Villager.Occupation occupation8 = Villager.Occupation.Unassigned;

    [HideInInspector]
    public int combinatorSize;

    public void LoadCombinator()
    {
        int count = 0;
        if (occupation1 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation1);
            count++;
        }
        if (occupation2 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation2);
            count++;
        }
        if (occupation3 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation3);
            count++;
        }
        if (occupation4 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation4);
            count++;
        }
        if (occupation5 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation5);
            count++;
        } 
        if (occupation6 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation6);
            count++;
        } 
        if (occupation7 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation7);
            count++;
        }
        if (occupation8 != Villager.Occupation.Unassigned)
        {
            allOccupations.Add(occupation8);
            count++;
        }
        combinatorSize = count;
    }
    
    public bool HasExactOccupations(List<Villager.Occupation> occupationList)
    {
        bool hasExact = true; 
        if (allOccupations.Count == occupationList.Count)
        {
            foreach (Villager.Occupation occ in occupationList)
            {
                if (allOccupations.Contains(occ) == false)
                    hasExact = false; 
            }
        }
        else
            hasExact = false;
        return hasExact;
    }  
    public bool IsCoveredByList(List<Villager.Occupation> occupationList)
    {
        bool isCovered = true;  

        foreach (Villager.Occupation occ in allOccupations)          
                if (occupationList.Contains(occ) == false)
            { 
                isCovered = false;

            }
        return isCovered;
    }

    public bool HasOccupation (Villager.Occupation occ)
    {
        if (occupation1 == occ)
            return true;
        if (occupation2 == occ)
            return true;
        if (occupation3 == occ)
            return true;
        if (occupation4 == occ)
            return true;
        if (occupation5 == occ)
            return true;
        if (occupation6 == occ)
            return true;
        if (occupation7 == occ)
            return true;
        if (occupation8 == occ)
            return true;
        return false;
    }
}
