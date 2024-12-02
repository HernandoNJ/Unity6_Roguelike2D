using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private PlayerController playerController;

    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get ; private set ; }

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
        TurnManager = new TurnManager();
        boardManager.Init();
        playerController.SpawnPlayer(boardManager, new Vector2Int(1,1));
    }
}
