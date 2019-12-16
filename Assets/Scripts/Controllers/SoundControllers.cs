using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControllers : MonoBehaviour
{
    public static SoundControllers Instance;

    public AudioSource fxAudio;
    public AudioSource bgAudio; 

    public AudioClip intro1Music;
    public AudioClip intro2Music;
    public AudioClip overworldMusic;
    public AudioClip endMusic; 

    public AudioClip heroDash;
    public AudioClip groupWalk;
    public AudioClip monsterWalk;
    public AudioClip monsterEat;
    public AudioClip dangerX;



    float groupSoundDelay = 0.5f;
    float monsterSoundDelay = 1.2f;
    bool playGroupMovement = false; 
    bool playMonsterMovement = false; 


    private void Awake()
    {
        Instance = this; 
    }
   
    public void StartGroupWalk()
    { 
    //    playGroupMovement = true;
    //    StartCoroutine(GroupMove());
    }
    public void StopGroupWalk()
    { 
   //     playGroupMovement = false;
    }
    public void StartMonsterWalk()
    {
   //     playMonsterMovement  = true; 
    //    StartCoroutine(MonsterMove());
    }
    public void StopMonsterWalk()
    { 
    //    playMonsterMovement = false;
    }
 
    IEnumerator GroupMove()
    {
        while (playGroupMovement)
        {
            fxAudio.PlayOneShot(groupWalk);
            yield return new WaitForSeconds(groupSoundDelay);
        }
    }
    IEnumerator MonsterMove()
    {
        while (playMonsterMovement)
        {
            fxAudio.PlayOneShot(monsterWalk);
            yield return new WaitForSeconds(monsterSoundDelay);
        }
    }
}
