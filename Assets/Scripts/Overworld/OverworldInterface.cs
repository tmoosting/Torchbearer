using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldInterface : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject progressPanel;
    public TextMeshProUGUI progressText;



    public void ClickSuccessYes()
    {
        OverworldController.Instance.FinishLevel(true);
        dialogBox.SetActive(false);
    }
    public void ClickSuccessNo()
    {
        OverworldController.Instance.FinishLevel(false);
        dialogBox.SetActive(false);

    }

    public void UpdateProgressPanel()
    {
        string str = "";

        foreach (int id in OverworldController.Instance.towerCompletion.Keys)
        {
            str += id + ": ";
            if (OverworldController.Instance.towerCompletion[id] == true)
                str += "PASS\n";
            else
                str += "FAIL\n";
        }
        progressText.text = str;
    }


}
