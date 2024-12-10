using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_CellVector;

    public virtual void Init(Vector2Int cellVector)
    {
        m_CellVector = cellVector;
    }
    
    public virtual void PlayerEntered() { }
    
    public virtual bool PlayerWantsToEnter() => true;
}