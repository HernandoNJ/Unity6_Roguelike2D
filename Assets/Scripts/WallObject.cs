using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile tile;
    public int maxHealth;

    private Tile m_OriginalTile;
    private int m_HealthPoints;

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);

        maxHealth = 3;
        m_HealthPoints = maxHealth;

        m_OriginalTile = m_BoardManager.GetCellTile(coord);
        m_BoardManager.SetCellTile(coord, tile);
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
        m_BoardManager.SetCellTile(m_ThisCellCoord, m_OriginalTile);
        Destroy(gameObject);
        Debug.Log("pwe is true");
        return true;
    }

}