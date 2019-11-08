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
    GameState currentGameState;

    public bool skipIntroduction;


    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        if (skipIntroduction == true)       
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
            UIController.Instance.ClearTitleScreen();
            SetGameState(GameState.Overworld);
        }
        if (state == GameState.Overworld)
        {
            
        }
    }


    public GameState GetGameState()
    {
        return currentGameState;
    }
}
