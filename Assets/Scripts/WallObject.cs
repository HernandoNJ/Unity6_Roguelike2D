using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile obstacleTile;
    public int maxHealth;

    private Tile m_OriginalTile;
    private int m_HealthPoints;

    public override void Init(Vector2Int cellVectorArg)
    {
        base.Init(cellVectorArg);

        maxHealth = 3;
        m_HealthPoints = maxHealth;

        m_OriginalTile = m_BoardManager.GetCellTile(cellVectorArg);
        m_BoardManager.SetCellTile(cellVectorArg, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoints -= 1;

        if (m_HealthPoints > 0)
        {
            Debug.Log("pwe is false");
            return false;
        }

        // if m_HealthPoints = 0
        m_BoardManager.SetCellTile(m_CellVector, m_OriginalTile);
        Destroy(gameObject);
        Debug.Log("pwe is true");
        return true;
    }

    public override int ShowPoints() => m_HealthPoints;
}