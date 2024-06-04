using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState {Run,WinGame,LoseGame};
    private GameState gameState;

    public GameState State { get { return gameState; } private set { gameState = value; }}

    public delegate void OnGameEvent();
    public event OnGameEvent onEndGame;
    public event OnGameEvent onStartGame;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UIPopupManager.Instance.messages.ShowToturialMessage(StartGame);
    }

    void Update()
    {
        
    }
    public void SetLoseGame()
    {
        UIPopupManager.Instance.messages.ShowLoseMessage();
        State = GameState.LoseGame;

        if (onEndGame != null)
            onEndGame();
    }

    public void StartGame()
    {
        State = GameState.Run;

        if (onStartGame != null)
            onStartGame();

        PlayerController.Instance.gameObject.SetActive(true);
    }

    public void SetWinGame()
    {
        UIPopupManager.Instance.messages.ShowWinMessage(StartGame);
        State = GameState.WinGame;

        if (onEndGame != null)
            onEndGame();

    }
}
