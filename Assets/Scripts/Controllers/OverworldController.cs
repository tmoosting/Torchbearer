using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    public static OverworldController Instance;

    [Header("Assign")]
    public List<GameObject> towerList = new List<GameObject>();
    public GameObject heroObject;

    int currentStage = 1;
    int numberOfStages = 5;
    [HideInInspector]
    public int chosenTowerID; 
    [HideInInspector]
    public Dictionary<int, bool> towerCompletion = new Dictionary<int, bool>();
    



    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetTowerIDs();
        SyncTowersToStage();
        //  foreach (Transform transform in towerHolder.GetComponentsInChildren<Transform>())
        //  {
        //     transform.gameObject.SetActive(false);
        //     transform.GetComponent<Tower>().towerActive = false;
        //  }
        //towerHolder.GetComponentsInChildren<Transform>()[0].gameObject.GetComponent<Tower>().towerActive = true;
        //towerHolder.GetComponentsInChildren<Transform>()[0].gameObject.SetActive(true); 
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
    void SyncTowersToStage()
    { 
        foreach (GameObject obj in towerList)        
            obj.GetComponent<Tower>().explorable = false;  

        if (currentStage <= numberOfStages)
        towerList[currentStage- 1].GetComponent<Tower>().explorable = true; 
    }

    public void FailStage()
    { 
        towerCompletion.Add(chosenTowerID, false); 
        ProgressStage();
    }
    public void SucceedStage()
    { 
        towerCompletion.Add(chosenTowerID, true); 
        ProgressStage();
    }
    void ProgressStage()
    {
        currentStage++;
        UIController.Instance.overworldInterface.UpdateProgressPanel();
        MoveOverworldElements();
        SyncTowersToStage();
    }

    void MoveOverworldElements()
    {
        heroObject.gameObject.transform.position += new Vector3(0,9,0) ;

    }
}
