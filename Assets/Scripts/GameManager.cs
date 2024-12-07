using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;

    public static GameManager Instance { get; private set; }

    public TurnHandler TurnHandler { get; private set; }

    private int m_FoodAmount = 100;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        TurnHandler = new TurnHandler();
        
        // When the OnTick event is triggered
        // Call the OnTickHandler method
        TurnHandler.OnTick += OnTickHandler;

        boardManager.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
    }

    // OnTurnHappen
    private void OnTickHandler()
    {
        m_FoodAmount -= 1;
        Debug.Log($"Current food {m_FoodAmount}");
    }
}