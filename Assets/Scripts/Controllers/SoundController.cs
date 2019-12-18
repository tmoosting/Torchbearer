using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

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
    public AudioClip dangerDodged;
    public AudioClip levelSuccessful;
    public AudioClip levelDoubleSuccessful;
    public AudioClip appearX;

    bool monsterMoving = false;
     

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
        if (monsterMoving == false)
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
        monsterMoving = true;
        fxAudio.PlayOneShot(monsterWalk);
        for (int i = 0; i < 45; i++)
            yield return null;
        fxAudio.PlayOneShot(monsterWalk);
        for (int i = 0; i < 45; i++)
            yield return null;
        fxAudio.PlayOneShot(monsterWalk);
        monsterMoving = false;
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

    public void PlayIntroMusic1()
    {
        bgAudio.volume = 0.2f;
        bgAudio.Stop();
        bgAudio.clip = intro1Music;
        bgAudio.Play();
    }
    public void PlayIntroMusic2()
    {
        bgAudio.volume = 0.8f;
        bgAudio.Stop();
        bgAudio.clip = intro2Music;
        bgAudio.Play();
    }
    public void PlayOverworldBackgroundMusic()
    {
        bgAudio.volume = 0.2f; 
        bgAudio.Stop();
        bgAudio.clip = overworldMusic;
        bgAudio.Play();
    }
    public void StopOverworldBackgroundMusic()
    { 
        bgAudio.Stop();
    }
    public void PlayEndMusic()
    {
        bgAudio.volume = 0.2f;
        bgAudio.Stop();
        bgAudio.clip = endMusic;
        bgAudio.Play();
    }
    public void PlayHeroMoves()
    {
        fxAudio.PlayOneShot(heroDash);

    }
    public void PlayMonsterEatsVillagerSound()
    {
            fxAudio.PlayOneShot(monsterEat);
    }
    public void PlayDangerDeath()
    {
          fxAudio.PlayOneShot(dangerX); 
    }
    public void PlayDangerDodged()
    {
        fxAudio.PlayOneShot(dangerDodged);
    }
    public void PlayLevelSuccessful()
    {
        fxAudio.PlayOneShot(levelSuccessful);
    }
    public void PlayLevelDoubleSuccessful()
    {
        fxAudio.PlayOneShot(levelDoubleSuccessful);
    }
    public void PlayXAppears()
    {
        fxAudio.PlayOneShot(appearX);
    }
}
