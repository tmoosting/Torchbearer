using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//whoopsie name
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
    public AudioClip crossAppears;
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
        //     playGroupMovement = true;
        //    StartCoroutine(GroupMove());
        // instead, play single shot three times with small delay
        StartCoroutine(PlayTripleGroupSound());
    }
    //public void StopGroupWalk()
    //{ 
    ////    playGroupMovement = false;  
    //}
    public void StartMonsterWalk()
    {
   //     playMonsterMovement  = true; 
  //   StartCoroutine(MonsterMove());
        StartCoroutine(PlayDoubleMonsterSound());
    }
  //  public void StopMonsterWalk()
  //  { 
  ////       playMonsterMovement = false;
  //  }
    IEnumerator PlayTripleGroupSound()
    {
        fxAudio.PlayOneShot(groupWalk);
        for (int i = 0; i < 20; i++)        
            yield return null;
        fxAudio.PlayOneShot(groupWalk);
        for (int i = 0; i < 20; i++)
            yield return null;
        fxAudio.PlayOneShot(groupWalk);  
    }
    IEnumerator PlayDoubleMonsterSound()
    {
        fxAudio.PlayOneShot(monsterWalk);
        for (int i = 0; i < 20; i++)
            yield return null;
        fxAudio.PlayOneShot(monsterWalk); 
    }
    //IEnumerator GroupMove()
    //{
    //    while (playGroupMovement)
    //    {
    //        Debug.Log("bool is " + playGroupMovement);
    //        fxAudio.PlayOneShot(groupWalk);
    //        yield return new WaitForSeconds(groupSoundDelay);
    //    }
    //}
    //IEnumerator MonsterMove()
    //{
    //    while (playMonsterMovement)
    //    {
    //        fxAudio.PlayOneShot(monsterWalk);
    //        yield return new WaitForSeconds(monsterSoundDelay);
    //    }
    //}

    public void PlayMonsterEatsVillagerSound()
    {
   //     fxAudio.PlayOneShot(monsterEat);
    }
    public void PlayCrossAppearSound()
    {
    //    fxAudio.PlayOneShot(crossAppears); 
    }
}
