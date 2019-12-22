using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Assign")]
    public Narrator narrator; 
    public GameObject titlePanel;
    public GameObject creditFadePanel;
    public GameObject transitionFadePanel;
    public TextMeshProUGUI creditsHeader;
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI creditsText2;
    public GameObject overworldHolder;

    private void Awake()
    {
        Instance = this;

        if (SceneController.Instance.madeTransition == false)
        {
            titlePanel.SetActive(false);
            creditFadePanel.SetActive(false);
            transitionFadePanel.SetActive(false);
            creditsHeader.gameObject.SetActive(false); 
            creditsText.gameObject.SetActive(false); 
            creditsText2.gameObject.SetActive(false); 
        } 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            if (GameController.Instance.GetGameState() == GameController.GameState.TitleScreen)
            {
                if (SceneController.Instance.madeTransition == false)
                {
                    ClearTitleScreen();
                    GameController.Instance.SetGameState(GameController.GameState.Introduction);
                }

            } 
        }
       
    }

    
   public void ShowTitleScreen()
    {
        titlePanel.SetActive(true);
    }
    public void ClearTitleScreen()
    {
        titlePanel.SetActive(false);
    }
    //public void ClearIntroduction()
    //{

    //}
    bool creditsStarted = false;
    public void FadeToCredits()
    {
        if (creditsStarted == false)
        {
            creditsStarted = true;
            OverworldController.Instance.GetNarrator().endEventPanel.SetActive(false);
            GameController.Instance.SetGameState(GameController.GameState.GameFinished);

            creditFadePanel.SetActive(true);
            StartCoroutine(FadeInBlackPanelForCredits());
            StartCoroutine(OverworldController.Instance.heroObject.GetComponent<Hero>().MoveHeroToEndMarker());
        }

    }

    IEnumerator FadeInBlackPanelForCredits()
    {
        Image bgImage = creditFadePanel.GetComponent<Image>();
        Color tempColor = bgImage.color;
        float alpha = 0f;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 6 )
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            bgImage.color = newColor;
            yield return null;
        }
        ShowCredits();
    }
    void ShowCredits()
    {
        creditsHeader.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(true);
        creditsText2.gameObject.SetActive(true);
    }
}
