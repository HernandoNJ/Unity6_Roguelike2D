using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public PlayerController playerController;

    private TurnManager m_TurnManager;

    private void Start()
    {
        m_TurnManager = new TurnManager();
      
        boardManager.Init();
        playerController.Spawn(boardManager, new Vector2Int(1,1));
    }
}
