using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState {Run,WinGame,LoseGame};
    private GameState gameState;

    public GameState State { get { return gameState; } private set { gameState = value; }}

    public delegate void OnEndGame();
    public event OnEndGame onEndGame;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UIPopupManager.Instance.messages.ShowToturialMessage();
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
}
