using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   
    public static SceneController Instance;

    public bool madeTransition = false;
    public bool lastLevelSuccess = false;

    private void Awake()
    {
        Instance = this; 
    }

    public void LoadLevel()
    {
        UIController.Instance.overworldHolder.SetActive(false);
        madeTransition = true;
        if (OverworldController.Instance.chosenTowerID >= 1)
        {
            SceneManager.LoadSceneAsync("Ice Level (test)");
        }
    }

    public void CompleteLevel()
    {
        UIController.Instance.overworldHolder.SetActive(true);
        lastLevelSuccess = true;
        SceneManager.LoadScene("Overworld");
        OverworldController.Instance.FinishLevel(true);
    }
    public void FailLevel()
    {
        UIController.Instance.overworldHolder.SetActive(true);
        lastLevelSuccess = false;
        SceneManager.LoadScene("Overworld");
        OverworldController.Instance.FinishLevel(false);
    }
}
