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
    Tower towerToBeChangedAfterLevel = null;

    int currentStage = 1;
    int numberOfStages = 4;
    [HideInInspector]
    public int chosenTowerID; 
    [HideInInspector] 
    bool proceedAllowed = true;
    [HideInInspector]
    public bool finalStageJustCompleted = false;
    bool postFinalStageMessageClickedAway = false;
    bool veryLastMessageClickedAway = false;
    bool levelSucceeded = false;
    [HideInInspector]
    public int monsterSteps = 0;
    GameObject markerToBeChanged;
    [HideInInspector]
    public bool villagerSpooked;
    bool villagerDangerDied;

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
    }

    bool IsTowerClickAllowed()
    {
        if (proceedAllowed == false)
            return false;
        if (UIController.Instance.narrator.EventPanelOpened() == true)
            return false;

        if (monster.isMoving == true)
            return false;

        if (groupObject.GetComponent<Group>().isMoving == true)
            return false;

        if (heroObject.GetComponent<Hero>().isMoving == true)
            return false;
        return true;
    }
    public void ClickTower(Tower tower)
    {
        if (IsTowerClickAllowed() == true)
        {
            towerToBeChangedAfterLevel = tower;
            chosenTowerID = tower.towerID;
            StartCoroutine(heroObject.GetComponent<Hero>().MoveHeroToTower(tower));
        }
    }

    public void FinishLevel(bool successful, bool withinTime)
    {
        villagerSpooked = false;
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
        SoundController.Instance.PlayOverworldBackgroundMusic();
        towerToBeChangedAfterLevel.SetCompletionSprite(); 
    }  
    public void SucceedStage(int monsterSteps)
    {
        this.monsterSteps = monsterSteps;
        levelSucceeded = true; 
        TakeSafeRoute(); 
    }
    public void FailStage()
    {
        this.monsterSteps = 2;
        levelSucceeded = false; 
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
        villagerDangerDied = false;
        markerToBeChanged = marker.gameObject;
        groupObject.GetComponent<Group>().MoveGroupToMarker(marker); 
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
       
        DangerMarker marker = potentialMarkerList[Random.Range(0, 3)];
        groupObject.GetComponent<Group>().MoveGroupToMarker(marker);
        markerToBeChanged = marker.gameObject;

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
            villagerDangerDied = false;
        }
        else // group has hit a danger
        {
            villagerDangerDied = true;
            VillageController.Instance.GroupHitsDanger(marker.containedDanger);
            narrator.OpenDangerHitEventPanel(marker.containedDanger);
        }
        monster.AddWaypoint(groupObject.transform.localPosition);
        
    }
    public void SetRipSprites()
    {
        if (villagerDangerDied && villagerSpooked)
            markerToBeChanged.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.dangerMarkerRipBoth;
        else if (villagerDangerDied)
            markerToBeChanged.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.dangerMarkerRipDanger;
        else if (villagerSpooked)
            markerToBeChanged.GetComponent<SpriteRenderer>().sprite = SpriteCollection.Instance.dangerMarkerRipGhost;
    }
    public void FinishFinalGroupMovement()
    {
        narrator.OpenEndEventPanel();
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

    public void EventPanelGotClosed(bool hasNextScreen)
    {

        if (finalStageJustCompleted == true)
        { 
            postFinalStageMessageClickedAway = true; 
            finalStageJustCompleted = false;
            if (hasNextScreen == false)
            {
                MoveGroupToEndPoint();
                postFinalStageMessageClickedAway = false;
                veryLastMessageClickedAway = true;
            }
        }
        else if (postFinalStageMessageClickedAway == true)
        {
            MoveGroupToEndPoint();
            postFinalStageMessageClickedAway = false;
            veryLastMessageClickedAway = true;
        }
        else if (veryLastMessageClickedAway == true)
        {
            UIController.Instance.FadeToCredits();
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
        {
            towerList[currentStage - 1].GetComponent<Tower>().explorable = true;
            towerList[currentStage - 1].GetComponent<Tower>().SetFlagSprite();

        }
    }
  
    public Narrator GetNarrator()
    {
        return narrator;
    }
  
}
