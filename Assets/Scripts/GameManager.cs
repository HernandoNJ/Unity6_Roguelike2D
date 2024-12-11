using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;
    public AudioSource audioSource;
    public Vector2Int initPlayerCell;
    public int m_CurrentLevel = 1;
    public int m_CurrentFood ;
    public int m_InitFood = 20;
    public int m_MaxFood = 50;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;

    public static GameManager Instance { get; private set; }
    public TurnHandler TurnHandler { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        { Destroy(gameObject); return; }

        Instance = this;
    }

    private void OnEnable()
    {
        // Initialize TurnHandler and subscribe to OnTick event
        TurnHandler = new TurnHandler();
        TurnHandler.OnTick += OnTickHandler;
    }

    private void OnDisable()
    {
        TurnHandler.OnTick -= OnTickHandler;
        TurnHandler = null;
    }

    private void Start()
    {
        InitializeUI();
        CheckReferences();
        StartNewGame();
    }

    private void InitializeUI()
    {
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
    }

    private void CheckReferences()
    {
        if (boardManager == null ||
            playerController == null ||
            UIDoc == null ||
            m_FoodLabel == null ||
            m_GameOverPanel == null ||
            m_GameOverMessage == null)
        {
            Debug.LogError(
                $"Reference missing in /* {nameof(CheckReferences)}() */  " +
                $"of  /* {GetType().Name} */ script");
        }
    }

    public void StartNewGame()
    {
        m_CurrentLevel = 1;
        
        // Reset food
        m_CurrentFood = m_InitFood;
        
        UpdateFoodLabel();
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        // Reset board and player
        playerController.Init();
        playerController.Spawn(boardManager, initPlayerCell);
        InitializeBoard();
        
        // Play background audio
        audioSource.Play();
    }

    public void StartNewLevel()
    {
        // Initialize the board and player for the new level
        InitializeBoard();
        playerController.Spawn(boardManager, initPlayerCell);

        m_CurrentLevel++;
    }

    private void InitializeBoard()
    {
        // Clear and set up the game board
        boardManager.CleanBoard();
        boardManager.Init();
    }

    private void UpdateFoodLabel()
    {
        m_FoodLabel.text = $"Food: {m_CurrentFood}";
    }

    private void OnTickHandler()
    {
        Debug.Log("Text in OnTickHandler");
        UpdateFood(-1);
    }

    public void UpdateFood(int amount)
    {
        m_CurrentFood += amount;
        if(m_CurrentFood >= m_MaxFood) m_CurrentFood = m_MaxFood;
        else if (m_CurrentFood < 0) m_CurrentFood = 0;
        
        UpdateFoodLabel();

        if (m_CurrentFood <= 0)
        {
            playerController.SetGameOver();
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var e in enemies)
                e.GetComponent<EnemyCellObject>().HandleGameOver();            
            
            //EnemyCellObject.Stop
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over! \n\nYou survived " + m_CurrentLevel + " levels";
        }
    }
}