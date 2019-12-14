using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   
    public static SceneController Instance;

    public bool madeTransition = false;
    public bool lastLevelSuccess = false;
    public bool lastLevelMonsterEvaded = false;


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
    public void EndLevel (bool levelCompleted, bool withinTimeLimit)
    {
      //  Debug.Log("ending with " + levelCompleted + " and timelimit " + withinTimeLimit);
        UIController.Instance.overworldHolder.SetActive(true);
        lastLevelSuccess = levelCompleted;
        lastLevelMonsterEvaded = withinTimeLimit;
        SceneManager.LoadScene("Overworld");
        OverworldController.Instance.FinishLevel(lastLevelSuccess, lastLevelMonsterEvaded);
    }

 
}
