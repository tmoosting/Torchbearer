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

    public void ClickTower(Tower tower)
    {
        OverworldController.Instance.chosenTowerID = tower.towerID;
        dialogBox.SetActive(true);
    }

    public void ClickSuccessYes()
    {
        OverworldController.Instance.SucceedStage();
        dialogBox.SetActive(false);
    }
    public void ClickSuccessNo()
    {
        OverworldController.Instance.FailStage();
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
