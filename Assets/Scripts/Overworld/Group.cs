﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public float movementSpeed;

    [HideInInspector]
    public bool isMoving = false;

    public void MoveGroupToMarker(DangerMarker marker)
    {  
        SoundController.Instance.StartGroupWalk();  
        StartCoroutine(MoveGroup(marker));
    }

    public IEnumerator MoveGroup(DangerMarker marker)
    {
        Vector3 markerPos = new Vector3(marker.gameObject.transform.localPosition.x, marker.gameObject.transform.localPosition.y , marker.gameObject.transform.localPosition.z);
        isMoving = true;
        int count = 0;
        
        while (gameObject.transform.localPosition != markerPos)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, markerPos, movementSpeed);
            if (count > 100000)
                break;
            yield return null;
        }
        //   SoundControllers.Instance.StopGroupWalk();
        isMoving = false;
        OverworldController.Instance.FinishGroupMovement(marker);
    }
    public IEnumerator MoveGroupToVector(Vector3 markerPos)
    { 
        int count = 0;

        while (gameObject.transform.localPosition != markerPos)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, markerPos, movementSpeed);
            if (count > 100000)
                break;
            yield return null;
        }
        OverworldController.Instance.FinishFinalGroupMovement( );
    }
}
