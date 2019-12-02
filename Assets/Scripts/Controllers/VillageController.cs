using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    public List<Villager> villagerList = new List<Villager>();

    

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }
    public void InitializeVillage()
    {
        CreateVillagers();

    }
    void CreateVillagers()
    {

    }

    public void GroupHitsDanger(Danger danger)
    {

    }
    public string GetFinalGroupString()
    {
        return "and so.. ";
    }
}
