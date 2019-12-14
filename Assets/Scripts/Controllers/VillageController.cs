using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    public List<Villager> villagerList = new List<Villager>();

    [HideInInspector]
    public Villager recentlyDeceasedVillager = null;
    public Villager recentlySpookedVillager = null;
    

    private void Awake()
    {
        Instance = this;
    }
    public void InitializeVillage()
    {
        foreach (Villager villager in villagerList)        
            villager.isAlive = true;
        
    }
    string CheckForCombinations()
    {
        string returnString = "";
        List<Villager.Occupation> deadLabors = new List<Villager.Occupation>();
        foreach (Villager deadVillager in GetDeceasedVillagers())        
            deadLabors.Add(deadVillager.occupation);

        if (deadLabors.Contains(Villager.Occupation.Courier))
            returnString += "Nobody will get their letters anymore!";

        if (deadLabors.Contains(Villager.Occupation.Farmer) && deadLabors.Contains(Villager.Occupation.Fisher))
            returnString += "people will get hungry";
        return returnString;
    }

    public string GetFinalGroupString()
    {
        string finalString = "";
        finalString += "The village will have to cope without their ";
        int counted = 0;
        foreach (Villager deadVillager in GetDeceasedVillagers())
        {
            counted++;
            finalString += deadVillager.occupation.ToString();
            if (counted < GetDeceasedVillagers().Count) 
                 finalString += ", ";
        }
        finalString += ".\n\n";

        finalString += CheckForCombinations();
        return finalString;
    }

    // select and kill a villager
    public void GroupHitsDanger(Danger danger)
    {
        recentlyDeceasedVillager =  GetRandomLivingVillager();
        recentlyDeceasedVillager.isAlive = false;
    }
    public void MonsterSpooksVillager()
    {
        recentlySpookedVillager = GetRandomLivingVillager();
        recentlySpookedVillager.isAlive = false;
        OverworldController.Instance.GetNarrator().OpenSpookedEventPanel();
    }
     
    public Villager GetRandomLivingVillager()
    {
        List<Villager> livingVillagers = new List<Villager>();
        foreach (Villager villager in villagerList)
            if (villager.isAlive == true)
                livingVillagers.Add(villager);
        return livingVillagers[Random.Range(0, livingVillagers.Count)];
    }

    List<Villager> GetDeceasedVillagers()
    {
        List<Villager> deceasedVillagers = new List<Villager>();
        foreach (Villager villager in villagerList)
            if (villager.isAlive == false)
                deceasedVillagers.Add(villager);
        return deceasedVillagers;
    }


}
