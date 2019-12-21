﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    public List<Villager> villagerList = new List<Villager>();

    [HideInInspector]
    public Villager recentlyDeceasedVillager = null;
    public Villager recentlySpookedVillager = null;

    public List<Combinator> combinatorList;
    public List<Combinator> sortedCombinatorList;



    private void Awake()
    {
        Instance = this;
    }
    public void InitializeVillage()
    {
        foreach (Villager villager in villagerList)        
            villager.isAlive = true;
        LoadCombinators();
        CheckForDuplicateCombinators();
        
    }
    void SortCombinatorsBySize()
    {
         sortedCombinatorList = new List<Combinator>();
        foreach (Combinator combinator in combinatorList)        
            if (combinator.combinatorSize ==  8)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  7)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  6)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  5)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  4)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  3)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  2)
                sortedCombinatorList.Add(combinator);
        foreach (Combinator combinator in combinatorList)
            if (combinator.combinatorSize ==  1)
                sortedCombinatorList.Add(combinator); 
    }
    void LoadCombinators()
    {
        foreach (Combinator combinator in combinatorList)        
            combinator.LoadCombinator();        
    }
    void CheckForDuplicateCombinators()
    {
        foreach (Combinator combinator in combinatorList)
        { 
            foreach (Combinator combinator2 in combinatorList)
                if (combinator != combinator2)
                { 
                    if (combinator.occupation1 == combinator2.occupation1)
                        if (combinator.occupation2 == combinator2.occupation2)
                            if (combinator.occupation3 == combinator2.occupation3)
                                if (combinator.occupation4 == combinator2.occupation4)
                                    if (combinator.occupation5 == combinator2.occupation5)
                                        if (combinator.occupation6 == combinator2.occupation6)
                                            if (combinator.occupation7 == combinator2.occupation7)
                                                if (combinator.occupation8 == combinator2.occupation8)
                                                    TagDoubleCombinators(combinator, combinator2);
                }
        }
    }
    void TagDoubleCombinators (Combinator comb1, Combinator comb2)
    { 
    }
    string CheckForCombinations()
    {
        SortCombinatorsBySize();
        List<Villager.Occupation> deadLabors = new List<Villager.Occupation>();
        foreach (Villager deadVillager in GetDeceasedVillagers())        
            deadLabors.Add(deadVillager.occupation);
         
        
        foreach (Combinator combinator in sortedCombinatorList)
        {
            if (combinator.HasExactOccupations(deadLabors))
            {
             //   Debug.Log("exavctmatch");
                return combinator.combinationString;
            } 
        }
        foreach (Combinator combinator in sortedCombinatorList)
        {
          //  Debug.Log("checkpartialmatch");

            if (combinator.IsCoveredByList(deadLabors))
            {
          //      Debug.Log("partialmatch");
                return combinator.combinationString;
            }
        }
        return "";
    }

    public string GetFinalGroupString()
    {
        string finalString = "";
        if (NoVillagersDied() == true)
        {
            finalString += "The night has cleared! New lands and lives await.\n\n";
            finalString += "Now, the villagers can rebuild their homes with spectacular capacity!\n\n";
            finalString += "And nobody died! It's a perfect ending to a minor mishap! ";
        }
        else
        {
            finalString += "The night has cleared! New lands and lives await.\n\n";
            finalString += "Now, the villagers can rebuild their homes in spectacular capacity!\n\n";
            finalString += "Without the help, however, of their \n\n";
            int counted = 0;
            foreach (Villager deadVillager in GetDeceasedVillagers())
            {
                counted++;
                finalString += deadVillager.occupation.ToString();
                if (GetDeceasedVillagers().Count == 1)
                    finalString += "";
                else if (GetDeceasedVillagers().Count == 2 && counted == GetDeceasedVillagers().Count - 1)
                    finalString += " and ";
                else if (counted < GetDeceasedVillagers().Count - 1)
                    finalString += ", ";
                else if (counted == (GetDeceasedVillagers().Count - 1) && counted > 2)
                    finalString += " and ";

           //     Debug.Log("occ "+ deadVillager.occupation.ToString() + "  counted: " + counted + " lsitcount: " + GetDeceasedVillagers().Count);
            }
            finalString += ".\n\n";

            finalString += CheckForCombinations();
        }
        

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
     
    bool NoVillagersDied()
    {
        foreach (Villager villager in villagerList)        
            if (villager.isAlive == false)
                return false;        
        return true;
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
