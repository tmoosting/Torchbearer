using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerController : MonoBehaviour
{
    public static DangerController Instance;
    List<Danger> dangerList = new List<Danger>();
    List<string> dangerNameList = new List<string>();
    List<string> dangerEventList = new List<string>();

    [Header("Strings")]
    public string danger1Name;
    public string danger1Event;
    public string danger2Name;
    public string danger2Event;
    public string danger3Name;
    public string danger3Event;
    public string danger4Name;
    public string danger4Event;
    public string danger5Name;
    public string danger5Event;
    public string danger6Name;
    public string danger6Event;
    public string danger7Name;
    public string danger7Event;
    public string danger8Name;
    public string danger8Event;
    public string danger9Name;
    public string danger9Event;
    public string danger10Name;
    public string danger10Event;

    private void Awake()
    {
        Instance = this;
 
    
    } 
    public void InitializeDangers()
    {
        dangerNameList.Add(danger1Name);
        dangerNameList.Add(danger2Name);
        dangerNameList.Add(danger3Name);
        dangerNameList.Add(danger4Name);
        dangerNameList.Add(danger5Name);
        dangerNameList.Add(danger6Name);
        dangerNameList.Add(danger7Name);
        dangerNameList.Add(danger8Name);
        dangerNameList.Add(danger9Name);
        dangerNameList.Add(danger10Name);
        dangerEventList.Add(danger1Event);
        dangerEventList.Add(danger2Event);
        dangerEventList.Add(danger3Event);
        dangerEventList.Add(danger4Event);
        dangerEventList.Add(danger5Event);
        dangerEventList.Add(danger6Event);
        dangerEventList.Add(danger7Event);
        dangerEventList.Add(danger8Event);
        dangerEventList.Add(danger9Event);
        dangerEventList.Add(danger10Event);
        CreateDangers();
        AddDangersToMarkers();
    }

    void CreateDangers()
    {
        for (int i = 0; i < 10; i++)
        {
            Danger danger = new Danger(dangerNameList[i], dangerEventList[i], SpriteCollection.Instance.dangerSpriteList[i]);
            dangerList.Add(danger);
        }
    }

    void AddDangersToMarkers()
    {
        int skipCount = 0;
        for (int i = 0; i < 15; i++)
        {
            DangerMarker marker = OverworldController.Instance.dangerMarkerList[i].GetComponent<DangerMarker>();
            if (marker.isSafe == true) 
                skipCount++; 
            else 
                marker.containedDanger = dangerList[i - skipCount];  
            //      Debug.Log(" marker " + i + " has danger with id " + marker.containedDanger.dangerID + " and text " + marker.containedDanger.dangerString);
        } 
    }
  

   

}
