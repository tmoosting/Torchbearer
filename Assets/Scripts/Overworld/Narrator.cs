using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Narrator : MonoBehaviour
{ 

    int currentStage = 0;
    bool eventPanelOpen = false;
    bool introductionDone = false;
    bool fastFinishCoroutine = false;

    [Header("Assigns")]
    public GameObject introPanel;
    public GameObject eventPanel;
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
    public TextMeshProUGUI eventPanelText;

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
            else if (eventPanelOpen == true )
                CloseEventPanel();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (introductionDone == true)
                ShowLastEventPanel();
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
        eventPanelImage.sprite = sprite;
        eventPanelText.text = text;
    }
    public void OpenEventPanel( string text)
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(false); 
        eventPanelText.text = text;
    }
    public void OpenSuccessfulLevelEventPanel()
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelImage.sprite = SpriteCollection.Instance.successfulLevelSprite;
        eventPanelText.text = successfulLevelString;
    }
    public void OpenDangerDodgedEventPanel()
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelImage.sprite = SpriteCollection.Instance.dangerDodgedSprite;
        eventPanelText.text = dangerDodgedString;
    }
    public void OpenDangerHitEventPanel (Danger danger)
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
        eventPanelImage.gameObject.SetActive(true);
        eventPanelImage.sprite = danger.dangerSprite;
        eventPanelText.text = danger.dangerString;
    }


    void ShowLastEventPanel()
    {
        eventPanelOpen = true;
        eventPanel.SetActive(true);
    }
    void CloseEventPanel()
    {
        eventPanelOpen = false;
        eventPanel.SetActive(false);
        OverworldController.Instance.EventPanelGotClosed();
    }


    // -------------------- INTRODUCTION
    public void StartIntroduction()
    {
        introPanel.SetActive(true);
        SetStage(1);
    }
    void FinishIntroduction()
    {
        introPanel.SetActive(false);
        UIController.Instance.ClearIntroduction();
        introductionDone = true;
        ClearIntroPanel();
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
                t = 1f;
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
                t = 1f;
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
}
