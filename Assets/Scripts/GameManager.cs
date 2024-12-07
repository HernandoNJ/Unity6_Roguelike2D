using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;
    public Vector2Int initPlayerPosition;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    
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
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        
        TurnHandler = new TurnHandler();
        
        // When the OnTick event is triggered
        // Call the OnTickHandler method
        TurnHandler.OnTick += OnTickHandler;

        boardManager.Init();
        playerController.Spawn(boardManager, initPlayerPosition);
    }

    // OnTurnHappen
    private void OnTickHandler()
    {
        m_FoodAmount -= 1;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
    }
}