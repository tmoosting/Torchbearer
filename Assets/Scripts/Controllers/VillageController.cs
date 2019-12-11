using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    public List<Villager> villagerList = new List<Villager>();

    [HideInInspector]
    public Villager recentlyDeceasedVillager = null;
    

    private void Awake()
    {
        Instance = this;
    }
   
    public void InitializeVillage()
    {
         
    }
    
    // select and kill a villager
    public void GroupHitsDanger(Danger danger)
    {
        recentlyDeceasedVillager =  GetRandomLivingVillager();
        recentlyDeceasedVillager.isAlive = false;
    } 
    public Villager GetRandomLivingVillager()
    {
        List<Villager> livingVillagers = new List<Villager>();
        foreach (Villager villager in villagerList)
            if (villager.isAlive == true)
                livingVillagers.Add(villager);
        return livingVillagers[Random.Range(0, livingVillagers.Count)];
    }
    public string GetFinalGroupString()
    {
        return "and so.. ";
    }
}
