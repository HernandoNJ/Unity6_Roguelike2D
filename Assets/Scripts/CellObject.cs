using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_CellVector;
    protected BoardManager m_BoardManager;

    private void OnEnable()
    {
        m_BoardManager = GameManager.Instance.boardManager;
    }

    public virtual void Init(Vector2Int cellVectorArg)
    {
        m_CellVector = cellVectorArg;
    }
    
    public virtual void PlayerEntered() { }
    
    public virtual bool PlayerWantsToEnter() => true;

    public virtual int ShowPoints() => 0;

}