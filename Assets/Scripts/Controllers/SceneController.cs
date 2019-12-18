using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   
    public static SceneController Instance;

    public bool madeTransition = false;
    public bool lastLevelSuccess = false;
    public bool lastLevelMonsterEvaded = false;

    public bool levelFullyLoaded = false; 

    private void Awake()
    {
        Instance = this; 
    }

    public void LoadLevel()
    {
        StartCoroutine(FadeToLevel()); 
    }
    IEnumerator FadeToLevel()
    {
        UIController.Instance.blackFadePanel.SetActive(true);
        Image bgImage = UIController.Instance.blackFadePanel.GetComponent<Image>();
        Color tempColor = bgImage.color;
        float alpha = 0f;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime *2)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            bgImage.color = newColor;
            yield return null;
        } 

        SoundController.Instance.StopOverworldBackgroundMusic();
        UIController.Instance.overworldHolder.SetActive(false);
        madeTransition = true;
        if (OverworldController.Instance.chosenTowerID == 1)
            SceneManager.LoadSceneAsync("Desert Level");
        else if (OverworldController.Instance.chosenTowerID == 2)
            SceneManager.LoadSceneAsync("Ice Level");
        else if (OverworldController.Instance.chosenTowerID == 3)
            SceneManager.LoadSceneAsync("Cave Level");
        else if (OverworldController.Instance.chosenTowerID == 4)
            SceneManager.LoadSceneAsync("Forest Level");
        StartCoroutine(FadeOutForLevel());
    }
    IEnumerator FadeOutForLevel()
    {
       
        while (levelFullyLoaded == false)
            yield return null;

        Image bgImage = UIController.Instance.blackFadePanel.GetComponent<Image>();
        Color tempColor = bgImage.color;
        float alpha = 0f;
        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime * 2)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            bgImage.color = newColor;
            yield return null;
        }
        UIController.Instance.blackFadePanel.SetActive(false);

    }

    public void EndLevel (bool levelCompleted, bool withinTimeLimit)
    {
        levelFullyLoaded = false; 
        StartCoroutine(FadeFromLevel (levelCompleted, withinTimeLimit));

        //  Debug.Log("ending with " + levelCompleted + " and timelimit " + withinTimeLimit);
  
    }
    IEnumerator FadeFromLevel(bool levelCompleted, bool withinTimeLimit)
    {
        UIController.Instance.blackFadePanel.SetActive(true);
        Image bgImage = UIController.Instance.blackFadePanel.GetComponent<Image>();
        Color tempColor = bgImage.color;
        float alpha = 0f;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 2)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            bgImage.color = newColor;
            yield return null;
        }
        UIController.Instance.overworldHolder.SetActive(true);
        lastLevelSuccess = levelCompleted;
        lastLevelMonsterEvaded = withinTimeLimit;
        SceneManager.LoadScene("Overworld"); 
        StartCoroutine(FadeOutForOverworld());
    }
    IEnumerator FadeOutForOverworld()
    {
        for (int i = 0; i < 80; i++)        
            yield return null;
        
        Image bgImage = UIController.Instance.blackFadePanel.GetComponent<Image>();
        Color tempColor = bgImage.color;
        float alpha = 0f;
        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime * 2)
        {
            Color newColor = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Lerp(alpha, 1, t));
            bgImage.color = newColor;
            yield return null;
        }
        UIController.Instance.blackFadePanel.SetActive(false); 
        OverworldController.Instance.FinishLevel(lastLevelSuccess, lastLevelMonsterEvaded); 
    }
}
