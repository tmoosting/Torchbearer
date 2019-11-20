using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    public static OverworldController Instance;

    [Header("Assign")] 
    public List<GameObject> towerList = new List<GameObject>();
    public List<GameObject> dangerMarkerList = new List<GameObject>();
    public GameObject heroObject;
    public GameObject monsterObject;
    public GameObject groupObject;

    int currentStage = 1;
    int numberOfStages = 5;
    [HideInInspector]
    public int chosenTowerID; 
    [HideInInspector]
    public Dictionary<int, bool> towerCompletion = new Dictionary<int, bool>();
    bool proceedAllowed = true;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetTowerIDs();
        SetMarkerTiers();
        SyncTowersToStage();
        SetSpritesFromCollection();
    }
    

    public void FinishLevel(bool successful)
    {
        Debug.Log("finishing level for stage.. " + currentStage);
        if (successful == true)
        {
            SucceedStage();
        }
        if (successful == false)
        {
            FailStage();
        }
    }

    public void ClickTower(Tower tower)
    {
        if (proceedAllowed == true)
        { 
            chosenTowerID = tower.towerID;
            StartCoroutine(heroObject.GetComponent<Hero>().MoveHeroToTower(tower));
        } 
    }
    public void SucceedStage()
    {
        towerCompletion.Add(chosenTowerID, true);
        TakeSafeRoute();
    }
    public void FailStage()
    {
        Debug.Log("add " + chosenTowerID);
        towerCompletion.Add(chosenTowerID, false);
        TakeRandomRoute();
    }


    void TakeSafeRoute()
    {
        proceedAllowed = false;
        DangerMarker marker = null;
        foreach (GameObject obj in dangerMarkerList)
        {
            DangerMarker objMarker = obj.GetComponent<DangerMarker>();
            if (objMarker.tier == currentStage)            
                if (objMarker.isSafe == true)
                    marker = objMarker;            
        }
        StartCoroutine(groupObject.GetComponent<Group>().MoveGroupToMarker(marker)); 
    }
    void TakeRandomRoute()
    {
        proceedAllowed = false;
        List<DangerMarker> potentialMarketList = new List<DangerMarker>();
        foreach (GameObject obj in dangerMarkerList)
        {
            DangerMarker objMarker = obj.GetComponent<DangerMarker>();
            if (objMarker.tier == currentStage)
                potentialMarketList.Add(objMarker);
        }  
        StartCoroutine(groupObject.GetComponent<Group>().MoveGroupToMarker(potentialMarketList[Random.Range(0, 3)]));
    }

    public void FinishGroupMovement (DangerMarker marker)
    {
        // TODO: show what happens with the group 
        ProgressStage();
    }

    void ProgressStage()
    {
        currentStage++;
        // UIController.Instance.overworldInterface.UpdateProgressPanel(); 
        SyncTowersToStage();
        proceedAllowed = true;
    }

    void SetTowerIDs()
    {
        int count = 0;
        foreach (GameObject obj in towerList)
        {
            count++;
            obj.GetComponent<Tower>().towerID = count;
        }
    }
    void SetMarkerTiers()
    {
        int count = 0;
        int tier = 1;
        foreach (GameObject obj in dangerMarkerList)
        {
            count++;
            if (count == 4)
            {
                count = 1;
                tier++;
            } 
            obj.GetComponent<DangerMarker>().tier = tier;
        }
    }
    void SyncTowersToStage()
    {
        foreach (GameObject obj in towerList)
            obj.GetComponent<Tower>().explorable = false;

        if (currentStage <= numberOfStages)
            towerList[currentStage - 1].GetComponent<Tower>().explorable = true;
    }
    void SetSpritesFromCollection()
    {
        heroObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.heroSprite;
        monsterObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.monsterSprite;
        groupObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.groupSprite;
        foreach (GameObject obj in towerList)
            obj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.towerSprite;
        foreach (GameObject obj in dangerMarkerList)
            obj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.dangerMarkerSprite;
    }
}
