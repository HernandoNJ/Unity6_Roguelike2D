using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;
    public Vector2Int initPlayerPosition;
    public int m_CurrentLevel = 1;
    public int m_FoodAmount = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    
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
        StartNewLevel();
        
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        
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

        if (m_FoodAmount <= 0)
        {
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over! \n\nYou traveled through " + m_CurrentLevel + " levels";
        }
    }

    public void StartNewLevel()
    {
        boardManager.CleanBoard();
        boardManager.Init();
        playerController.Spawn(boardManager, initPlayerPosition);
        m_CurrentLevel++;
    }
}