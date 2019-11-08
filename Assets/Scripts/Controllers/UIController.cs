using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Assign")]
    public OverworldInterface overworldInterface;
    public GameObject titlePanel;
   

    private void Awake()
    {
        Instance = this;
        titlePanel.SetActive(false);
        overworldInterface.dialogBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameController.Instance.GetGameState() == GameController.GameState.TitleScreen)
                GameController.Instance.SetGameState(GameController.GameState.Introduction);
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



     
}
