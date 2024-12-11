using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_ThisCellCoord;
    protected BoardManager m_BoardManager;

    private void OnEnable()
    {
        m_BoardManager = GameManager.Instance.boardManager;
    }

    public virtual void Init(Vector2Int coord)
    {
        m_ThisCellCoord = coord;
    }
    
    public virtual void PlayerEntered() { }
    
    public virtual bool PlayerWantsToEnter() => true;

}