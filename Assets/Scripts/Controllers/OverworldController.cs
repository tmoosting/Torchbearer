using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    public static OverworldController Instance;
    Narrator narrator;

   [Header("Assign")] 
    public List<GameObject> towerList = new List<GameObject>();
    public List<GameObject> dangerMarkerList = new List<GameObject>();
    public GameObject heroObject;
    public Monster monster;
    public GameObject groupObject;
    public GameObject endMarker; 

    int currentStage = 1;
    int numberOfStages = 4;
    [HideInInspector]
    public int chosenTowerID; 
    [HideInInspector]
    public Dictionary<int, bool> towerCompletion = new Dictionary<int, bool>();
    bool proceedAllowed = true;
    [HideInInspector]
    public bool finalStageJustCompleted = false;
    bool postFinalStageMessageClickedAway = false;
    bool veryLastMessageClickedAway = false;
    bool levelSucceeded = false;
    [HideInInspector]
    public int monsterSteps = 0;

    

    private void Awake()
    {
        Instance = this;
    } 
    public void InitializeOverworld()
    {
        narrator = UIController.Instance.narrator;
        SetTowerIDs();
        SetMarkerTiers();
        SyncTowersToStage();
        SetSpritesFromCollection();
    }

    public void ClickTower(Tower tower)
    {
        if (proceedAllowed == true && UIController.Instance.narrator.EventPanelOpened() == false)
        {
            chosenTowerID = tower.towerID;
            StartCoroutine(heroObject.GetComponent<Hero>().MoveHeroToTower(tower));
        }
    }

    public void FinishLevel(bool successful, bool withinTime)
    { 
        if (successful == true)
        {
            if (withinTime == true)
                SucceedStage(0);
            else
                SucceedStage(1);
        }
        if (successful == false)
        {
            FailStage();
        }
    }  
    public void SucceedStage(int monsterSteps)
    {
        this.monsterSteps = monsterSteps;
        levelSucceeded = true;
        towerCompletion.Add(chosenTowerID, true);
        TakeSafeRoute(); 
    }
    public void FailStage()
    {
        this.monsterSteps = 2;
        levelSucceeded = false;
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
        List<DangerMarker> potentialMarkerList = new List<DangerMarker>();
        foreach (GameObject obj in dangerMarkerList)
        {
            DangerMarker objMarker = obj.GetComponent<DangerMarker>();
            if (objMarker.tier == currentStage)
                potentialMarkerList.Add(objMarker);
        }  
        StartCoroutine(groupObject.GetComponent<Group>().MoveGroupToMarker(potentialMarkerList[Random.Range(0, 3)]));
    }

    public void FinishGroupMovement (DangerMarker marker)
    {
        // TODO: show what happens with the group 
        ProgressStage();
        if (levelSucceeded == true) // group moves to safe marker because beacon is lit
        {
          narrator.OpenSuccessfulLevelEventPanel();
        }
        else if(marker.isSafe == true) // group moves to safe marker by chance
        {
           narrator.OpenDangerDodgedEventPanel(); 
        }
        else // group has hit a danger
        {
            VillageController.Instance.GroupHitsDanger(marker.containedDanger);
            narrator.OpenDangerHitEventPanel(marker.containedDanger);
        }
        monster.AddWaypoint(groupObject.transform.localPosition); 
    }

    public void FinishFinalGroupMovement()
    {
        narrator.OpenEventPanel(VillageController.Instance.GetFinalGroupString());
    }
    void ProgressStage()
    {
        currentStage++;
        // UIController.Instance.overworldInterface.UpdateProgressPanel(); 
        SyncTowersToStage();         
        proceedAllowed = true;
        if (currentStage > numberOfStages)
            finalStageJustCompleted = true;
    }

    public void EventPanelGotClosed()
    {
        if (finalStageJustCompleted == true)
        {
            narrator.OpenEndEventPanel();

            postFinalStageMessageClickedAway = true;

            finalStageJustCompleted = false;
        }
        else if (postFinalStageMessageClickedAway == true)
        {
            MoveGroupToEndPoint();
            postFinalStageMessageClickedAway = false;
            veryLastMessageClickedAway = true;
        }
        else if (veryLastMessageClickedAway == true)
        {
            UIController.Instance.FadeToBlack();
        }

    }


    void MoveGroupToEndPoint()
    {
        StartCoroutine(groupObject.GetComponent<Group>().MoveGroupToVector(endMarker.transform.localPosition));
    }



    // ------------- INIT Stuff

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
      //  heroObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.heroSprite;
      //  monsterObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.monsterSprite;
        groupObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.groupSprite;
        foreach (GameObject obj in towerList)
            obj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.towerSprite;
        foreach (GameObject obj in dangerMarkerList)
            obj.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.dangerMarkerSprite;
    }
    public Narrator GetNarrator()
    {
        return narrator;
    }
}
