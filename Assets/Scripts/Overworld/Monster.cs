using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float movementSpeed;

    int stepsRemaining = 0;

   
    public List<Vector3> waypointList = new List<Vector3>();
    int stepsCompleted  = 0; // 0 is start, 1 is group start pos, 2 is tier1, 3 is tier2 etc

    private void Start()
    {
        waypointList.Add(OverworldController.Instance.groupObject.transform.localPosition);
    }

    public void AddWaypoint(Vector3 givenVector)
    {
        waypointList.Add(givenVector);
    }

    public void MoveForward( )
    {
        stepsRemaining = OverworldController.Instance.monsterSteps; 
        if (stepsRemaining > 0 )
         MoveMonsterToVector(waypointList[stepsCompleted ]); 
    }
    void FinishOneMovement()
    {
        
        stepsCompleted++;
        stepsRemaining--;
        if (MonsterAtGroup() == true)
            VillageController.Instance.MonsterSpooksVillager();
        else if (stepsRemaining > 0 && OverworldController.Instance.finalStageJustCompleted == false && stepsCompleted < 5)
            MoveMonsterToVector(waypointList[stepsCompleted]);
    }
    public void MoveMonsterToVector(Vector3 targetVector)
    {
        //  Debug.Log("moving mnster for steps: "   + " to x: " + waypointList[stepsCompleted  ].x + " and y: "+ waypointList[stepsCompleted  ].y);

        SoundControllers.Instance.StartMonsterWalk();
        StartCoroutine(MoveMonster(targetVector));
    }

    public IEnumerator MoveMonster (Vector3 targetVector)
    {
        //  Debug.Log("moving mnster for steps: "   + " to x: " + waypointList[stepsCompleted  ].x + " and y: "+ waypointList[stepsCompleted  ].y);
         
        int count = 0; 
        while (gameObject.transform.localPosition != targetVector)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, targetVector, movementSpeed);
            if (count > 100000)
                break;
            yield return null;
        }
        SoundControllers.Instance.StopMonsterWalk();
        FinishOneMovement();
    }

    bool MonsterAtGroup()
    {
        if (gameObject.transform.localPosition == OverworldController.Instance.groupObject.transform.localPosition)
            return true;
        else
            return false; 
    }
}
