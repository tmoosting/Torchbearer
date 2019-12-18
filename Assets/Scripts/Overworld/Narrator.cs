﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Narrator : MonoBehaviour
{ 

    int currentStage = 0;
    bool eventPanelOpen = false;
    bool dangerEventPanelOpen = false;
    bool levelSuccessfulEventPanelOpen = false;
    bool dangerDodgedPanelOpen = false;
    bool spookedEventPanelOpen = false;
    bool endEventPanelOpen = false;
    bool deadVillagerEventPanelOpen = false;
    bool introductionDone = false;
    bool fastFinishCoroutine = false;
    bool waitingForCross = false;

    [Header("Assigns")]
    public GameObject introPanel;
    public GameObject eventPanel;
    public GameObject endEventPanel;
    public GameObject screen1Holder;
    public GameObject screen2Holder;
    public GameObject screen3Holder;
    public GameObject screen4Holder;

    public Image screen1Image;
    public TextMeshProUGUI screen1Text;

    public Image screen2Image1;
    public Image screen2Image2;
    public TextMeshProUGUI screen2Text1;
    public TextMeshProUGUI screen2Text2;

    public Image screen3Image1;
    public Image screen3Image2;
    public TextMeshProUGUI screen3Text1;
    public TextMeshProUGUI screen3Text2;

    public Image screen4Image;
    public TextMeshProUGUI screen4Text;

    public Image eventPanelImage;
    public Image eventPanelCrossImage;
    public TextMeshProUGUI eventPanelText;
    public TextMeshProUGUI endEventPanelText;

    [Header("Intro General")]
    public float transitionDelay; // in seconds

    [Header("Intro Screen 1")]
    public string screen1String;
    public float screen1ImageAppearSpeed; 

    [Header("Intro Screen 2")]
    public string screen2String1;
    public string screen2String2;
    public float screen2Image1AppearSpeed;
    public float screen2Text1AppearDelay; // in seconds
    public float screen2TransitionTime;
    public float screen2Image2AppearSpeed;
    public float screen2Text2AppearDelay;

    [Header("Intro Screen 3")]
    public string screen3String1;
    public string screen3String2;
    public float screen3Image1AppearSpeed;
    public float screen3Text1AppearDelay; // in seconds
    public float screen3TransitionTime;
    public float screen3Image2AppearSpeed;
    public float screen3Text2AppearDelay;

    [Header("Intro Screen 4")] 
    public string screen4String;
    public float screen4ImageAppearSpeed;
    public float screen4TextAppearDelay;  // in seconds

    [Header("Event Panel")]
    public string heroStandsUpString;
    public string successfulLevelString;
    public string dangerDodgedString;

    [Header("End")]
    public string stage5CompletedString;


    private void Start()
    {
        introPanel.SetActive(false);
        eventPanel.SetActive(false);
        ClearIntroPanel();
        LoadContentIntoScreens(); 
    }  
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentStage > 0 && currentStage < 5)
            {
                fastFinishCoroutine = true;
         //       SetStage(currentStage += 1);  //TODO: Make a click first complete the current screen, then proceed on next click
            }
            if (dangerEventPanelOpen == true )
                CloseDangerEventPanel();
            else if (spookedEventPanelOpen == true)
                CloseSpookedEventPanel();
            else if (deadVillagerEventPanelOpen == true && waitingForCross  == false)       
                CloseDeadVillagerEventPanel();
            else if (dangerDodgedPanelOpen == true)
                CloseDangerDodgedPanel();
            else if (eventPanelOpen == true )
                CloseEventPanel();      
            else if ( levelSuccessfulEventPanelOpen == true)
                CloseLevelClearedEventPanel();    
            else if (endEventPanelOpen == true)
                UIController.Instance.FadeToBlack(); 

            
        }
        if (Input.GetMouseButtonDown(1))
        {
            //if (introductionDone == true)
            //    ShowLastEventPanel();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            if ( GameController.Instance.GetGameState() == GameController.GameState.Introduction)
                  SkipRestOfIntroduction();
        }
    } 
    public bool EventPanelOpened()
    {
        return eventPanelOpen;
    }
    // -------------------- EVENTPANEL

   public void OpenEventPanel(Sprite sprite, string text)
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);  
        eventPanelImage.sprite = sprite;
        eventPanelText.text = text;
    }
    public void OpenEventPanel( string text)
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(false); 
        eventPanelCrossImage.gameObject.SetActive(false);
        eventPanelText.text = text;
    }  
    public void OpenEndEventPanel()
    {
        endEventPanelOpen = true;
        endEventPanel.SetActive(true); 
        endEventPanelText.text = VillageController.Instance.GetFinalGroupString();
    }
    
    public void OpenSuccessfulLevelEventPanel()
    {
      
        if (SceneController.Instance.lastLevelMonsterEvaded == true)
            SoundController.Instance.PlayLevelDoubleSuccessful();
        else
            SoundController.Instance.PlayLevelSuccessful();
        levelSuccessfulEventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);
        eventPanelImage.sprite = SpriteCollection.Instance.successfulLevelSprite;
        eventPanelText.text = successfulLevelString;
    }
    public void OpenDangerDodgedEventPanel()
    {
        SoundController.Instance.PlayDangerDodged();
        dangerDodgedPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);
        eventPanelImage.sprite = SpriteCollection.Instance.dangerDodgedSprite;
        eventPanelText.text = dangerDodgedString;
    }
    public void OpenDangerHitEventPanel (Danger danger)
    {
        SoundController.Instance.PlayDangerDeath();
        dangerEventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);
        eventPanelImage.sprite = danger.dangerSprite;
        string str = "";
        str += danger.dangerID;
        str += "\n";
        str += danger.dangerString;
        str += "\n\n";
        
        eventPanelText.text = str;
    }
    public void OpenSpookedEventPanel(   )
    {
        spookedEventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);
        eventPanelImage.sprite = SpriteCollection.Instance.spookedSprite;
        string str = "";
        str += "A villager got spooked!"; 
        str += "\n\n"; 
        eventPanelText.text = str;
        SoundController.Instance.PlayMonsterEatsVillagerSound();
    }
    public void OpenDeadVillagerEventPanel(   )
    { 
        deadVillagerEventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelCrossImage.gameObject.SetActive(false);
        string str = "";
        if (spookException == false)
        {
            eventPanelImage.sprite = VillageController.Instance.recentlyDeceasedVillager.sprite;
            str += VillageController.Instance.recentlyDeceasedVillager.villagerID;
        }
        else
        {
            eventPanelImage.sprite = VillageController.Instance.recentlySpookedVillager.sprite;
            str += VillageController.Instance.recentlySpookedVillager.villagerID;
        } 
        str += "\n";
        str += "has met their demise";
        str += "\n\n"; 
        eventPanelText.text = str;
        waitingForCross = true;
        StartCoroutine(ShowCrossAfterDelay());
    }
    IEnumerator ShowCrossAfterDelay()
    {

        for (int i = 0; i < 50; i++)
        {
            if (i == 48)
                SoundController.Instance.PlayXAppears();
            yield return null;
        }
        waitingForCross = false;
      
        eventPanelCrossImage.gameObject.SetActive(true);
    }
    //public void OpenEndEventPanel(  )
    //{
    //    SoundController.Instance.PlayEndMusic();
    //    eventPanelOpen = true;
    //    eventPanel.SetActive(true);
    //    eventPanelImage.gameObject.SetActive(true);
    //    eventPanelCrossImage.gameObject.SetActive(false);
    //    eventPanelImage.sprite = SpriteCollection.Instance.endSprite;
    //    string str = "";
    //    str += " run to safety..";
    //    eventPanelText.text = str;
    //}
    void ShowLastEventPanel()
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
    }
    void CloseEventPanel()
    {
    //    Debug.Log("close vent normal");
        eventPanelOpen = false;
        levelSuccessfulEventPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(true);
    }
    void CloseLevelClearedEventPanel()
    {
        //    Debug.Log("close vent normal");
        levelSuccessfulEventPanelOpen = false; 
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(true);
        if (SceneController.Instance.lastLevelMonsterEvaded == false)
            OverworldController.Instance.monster.MoveForward();
    }
    void CloseDangerEventPanel()
    {
        dangerEventPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(true);
        OpenDeadVillagerEventPanel(); 
    }
    void CloseDangerDodgedPanel()
    {
        OverworldController.Instance.monster.MoveForward();
        dangerDodgedPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(false);
        
    }
    void CloseDeadVillagerEventPanel()
    {
        //   Debug.Log("close vent danger");
        if (spookException == true)
        {
            spookException = false;
        }
        else
        {
            OverworldController.Instance.monster.MoveForward();
            spookException = false;
        } 
          
        deadVillagerEventPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(false);
    }
    
    bool spookException = false;
    void CloseSpookedEventPanel()
    { 
        spookedEventPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed(true);
        spookException = true;
        OpenDeadVillagerEventPanel(); 
    }
    // -------------------- INTRODUCTION
    public void StartIntroduction()
    {
        introPanel.SetActive(true);
        SetStage(1);
    }
    void SkipRestOfIntroduction()
    {
        GameController.Instance.introCompleted = true;
        introPanel.SetActive(false); 
        introductionDone = true;
        ClearIntroPanel();
        SoundController.Instance.PlayOverworldBackgroundMusic();
    }
    void FinishIntroduction()
    {
        GameController.Instance.introCompleted = true;
        introPanel.SetActive(false);
        OpenEventPanel(SpriteCollection.Instance.heroStandsUpSprite, heroStandsUpString);
        introductionDone = true;
        ClearIntroPanel();
        SoundController.Instance.PlayOverworldBackgroundMusic();
    }
    void LoadContentIntoScreens()
    {
        screen1Image.sprite = SpriteCollection.Instance.screen1Sprite;
        screen2Image1.sprite = SpriteCollection.Instance.screen2Sprite1;
        screen2Image2.sprite = SpriteCollection.Instance.screen2Sprite2;
        screen3Image1.sprite = SpriteCollection.Instance.screen3Sprite1;
        screen3Image2.sprite = SpriteCollection.Instance.screen3Sprite2;
        screen4Image.sprite = SpriteCollection.Instance.screen4Sprite;
        screen1Text.text = screen1String;
        screen2Text1.text = screen2String1;
        screen2Text2.text = screen2String2;
        screen3Text1.text = screen3String1;
        screen3Text2.text = screen3String2;
        screen4Text.text = screen4String;
    }
    void SetStage (int stage)
    { 
        currentStage = stage;
        ClearIntroPanel();
        if (introductionDone == false) // hacky check yo stop coroutines running when click-through
        {
            if (stage == 1)
            {
                SoundController.Instance.PlayIntroMusic1();
                PrepScreen1();
                StartCoroutine(Screen1Routine());
            }
            if (stage == 2)
            {
                PrepScreen2();
                StartCoroutine(Screen2Routine());
            }
            if (stage == 3)
            {
                SoundController.Instance.PlayIntroMusic2();
                PrepScreen3();
                StartCoroutine(Screen3Routine());
            }
            if (stage == 4)
            {
                PrepScreen4();
                StartCoroutine(Screen4Routine());
            }
            if (stage == 5)
            { 
                FinishIntroduction();
            }
        } 
    } 
    void ClearIntroPanel()
    {
        fastFinishCoroutine = false;
        screen1Holder.SetActive(false);
        screen2Holder.SetActive(false);
        screen3Holder.SetActive(false);
        screen4Holder.SetActive(false);
        StopCoroutine(Screen1Routine());
        StopCoroutine(Screen2Routine());
        StopCoroutine(Screen3Routine());
        StopCoroutine(Screen4Routine());
    }
    void PrepScreen1()
    {
        screen1Holder.SetActive(true);
        screen1Text.gameObject.SetActive(false);
        SetImageAlphaTo0(screen1Image);
    }
    void PrepScreen2()
    {
        screen2Holder.SetActive(true);
        screen2Text1.gameObject.SetActive(false);
        screen2Text2.gameObject.SetActive(false);
        SetImageAlphaTo0(screen2Image1);
        SetImageAlphaTo0(screen2Image2);
    }
    void PrepScreen3()
    {
        screen3Holder.SetActive(true);
        screen3Text1.gameObject.SetActive(false);
        screen3Text2.gameObject.SetActive(false);
        SetImageAlphaTo0(screen3Image1);
        SetImageAlphaTo0(screen3Image2);
    }
    void PrepScreen4()
    {
        screen4Holder.SetActive(true);
        screen4Text.gameObject.SetActive(false);
        SetImageAlphaTo0(screen4Image);
    }
    IEnumerator Screen1Routine()
    { 
        screen1Text.gameObject.SetActive(true); 
        // Fade in Image
        Color tempColor = screen1Image.color;
        float alpha = screen1Image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen1ImageAppearSpeed)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            screen1Image.color = newColor;
            if (fastFinishCoroutine == true)
                t = 1f;
            yield return null;
        } 
        // Wait for general transition delay
        for (float t = 0.0f; t < transitionDelay; t += Time.deltaTime  )
        {
            if (fastFinishCoroutine == true)
                t = transitionDelay;
            yield return null;
        }
        if (introductionDone == false)
          SetStage(2); 
    } 
    IEnumerator Screen2Routine()
    {
        screen2Text1.gameObject.SetActive(true); 
        // Fade in Image
        Color tempColor1 = screen2Image1.color;
        float alpha1 = screen2Image1.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen2Image1AppearSpeed)
        {
            Color newColor = new Color(tempColor1.r, tempColor1.g, tempColor1.b, Mathf.Lerp(alpha1, 1, t));
            screen2Image1.color = newColor;
            if (fastFinishCoroutine == true)
                t = 1f;
            yield return null;
        } 
        // Wait for in-screen transition time
        for (float t = 0.0f; t < screen2TransitionTime; t += Time.deltaTime)
        {
            if (fastFinishCoroutine == true)
                t = screen2TransitionTime;
            yield return null;
        }
        screen2Text2.gameObject.SetActive(true);
        // Fade in Image
        Color tempColor2 = screen2Image2.color;
        float alpha2 = screen2Image2.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen2Image2AppearSpeed)
        {
            Color newColor = new Color(tempColor2.r, tempColor2.g, tempColor2.b, Mathf.Lerp(alpha2, 1, t));
            screen2Image2.color = newColor;
            if (fastFinishCoroutine == true)
            {
                fastFinishCoroutine = false; 
            }
            yield return null;
        } 
        for (float t = 0.0f; t < transitionDelay; t += Time.deltaTime)
        {
            if (fastFinishCoroutine == true)
                t = transitionDelay;
            yield return null;
        }
        if (introductionDone == false)
            SetStage(3);
    }
    IEnumerator Screen3Routine()
    {
        screen3Text1.gameObject.SetActive(true);
        // Fade in Image
        Color tempColor1 = screen3Image1.color;
        float alpha1 = screen3Image1.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen3Image1AppearSpeed)
        {
            Color newColor = new Color(tempColor1.r, tempColor1.g, tempColor1.b, Mathf.Lerp(alpha1, 1, t));
            screen3Image1.color = newColor;
            if (fastFinishCoroutine == true)
                t = 1f;
            yield return null;
        }
        // Wait for in-screen transition time
        for (float t = 0.0f; t < screen3TransitionTime; t += Time.deltaTime)
        {
            if (fastFinishCoroutine == true)
                t = screen3TransitionTime;
            yield return null;
        }
        screen3Text2.gameObject.SetActive(true);
        // Fade in Image
        Color tempColor2 = screen3Image2.color;
        float alpha2 = screen3Image2.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen2Image2AppearSpeed)
        {
            Color newColor = new Color(tempColor2.r, tempColor2.g, tempColor2.b, Mathf.Lerp(alpha2, 1, t));
            screen3Image2.color = newColor;
            if (fastFinishCoroutine == true)
            {
                fastFinishCoroutine = false; 
            }
            yield return null;
        }
        for (float t = 0.0f; t < transitionDelay; t += Time.deltaTime)
        {
            if (fastFinishCoroutine == true)
                t = transitionDelay;
            yield return null;
        }
        if (introductionDone == false)
            SetStage(4);
    }
    IEnumerator Screen4Routine()
    {
        screen4Text.gameObject.SetActive(true);
        // Fade in Image
        Color tempColor = screen4Image.color;
        float alpha = screen4Image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * screen4ImageAppearSpeed)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            screen4Image.color = newColor;
            if (fastFinishCoroutine == true)
                t = 1f;
            yield return null;
        }
        // Wait for general transition delay
        for (float t = 0.0f; t < transitionDelay; t += Time.deltaTime)
        {
            if (fastFinishCoroutine == true)
                t = transitionDelay;
            yield return null;
        }
        if (introductionDone == false)
            SetStage(5);
    }
    void SetImageAlphaTo0 (Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
    }
    public bool IsNextLevelClickAllowed()
    {
        if (eventPanelOpen || dangerEventPanelOpen || dangerDodgedPanelOpen || spookedEventPanelOpen || deadVillagerEventPanelOpen || waitingForCross)
            return false;
        return true;
    }

 
}
