using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public enum GameState { TitleScreen, Introduction, Overworld, Level, LevelFinished, GameFinished}

    GameState previousGameState;
    GameState currentGameState = GameState.TitleScreen;

    public bool skipIntroduction;
    [HideInInspector]
    public bool introCompleted = false;
  

    private void Start()
    {
        if (SceneController.Instance.madeTransition == false)
            StartGame();
        else
            currentGameState = GameState.Overworld;
    }

    void StartGame()
    { 
        VillageController.Instance.InitializeVillage();
        DangerController.Instance.InitializeDangers();
        OverworldController.Instance.InitializeOverworld();
        if (skipIntroduction == true || introCompleted == true)       
           SetGameState(GameState.Overworld);         
        else
           SetGameState(GameState.TitleScreen);
    }

    public void SetGameState (GameState state)
    {
        previousGameState = currentGameState;
        currentGameState = state;
        if (state == GameState.TitleScreen)
        {
            UIController.Instance.ShowTitleScreen();
        }
        if (state  == GameState.Introduction)
        {
            UIController.Instance.narrator.StartIntroduction(); 
        }
        if (state == GameState.Overworld)
        {
            
        }
    }
    public GameState GetGameState()
    {
        return currentGameState;
    }

    public void TransitionToLevel()
    {
        //TODO: make a small delay and nice fade to level, then switch scene
        //   UIController.Instance.overworldInterface.dialogBox.SetActive(true);
        SceneController.Instance.LoadLevel();
    }
    public void TransitionFromLevel()
    {
        //TODO: make a nice fade to overworld, and switch scene
    
    }

}
