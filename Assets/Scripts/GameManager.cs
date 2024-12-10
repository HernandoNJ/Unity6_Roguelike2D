using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;
    public Vector2Int initPlayerPosition;
    public int m_CurrentLevel = 1;

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
        SetNewLevel();
        
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        TurnHandler = new TurnHandler();

        // When the OnTick event is triggered
        // Call the OnTickHandler method
        TurnHandler.OnTick += OnTickHandler;
    }

    // OnTurnHappen
    private void OnTickHandler()
    {
        UpdateFoodAmount(-1);
    }

    public void UpdateFoodAmount(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
    }

    public void SetNewLevel()
    {
        boardManager.CleanBoard();
        boardManager.Init();
        playerController.Spawn(boardManager, initPlayerPosition);
        m_CurrentLevel++;
    }
}