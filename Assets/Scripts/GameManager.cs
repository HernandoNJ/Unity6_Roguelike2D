using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;

    public static GameManager Instance { get; private set; }

    public TurnHandler TurnHandler { get; private set; }

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

        boardManager.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
    }
}