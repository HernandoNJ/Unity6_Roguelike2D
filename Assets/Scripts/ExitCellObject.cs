using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject:CellObject
{
    public Tile exitTile;

    public override void Init(Vector2Int cellVectorArg)
    {
        base.Init(cellVectorArg);
        m_BoardManager.SetCellTile(cellVectorArg, exitTile);
    }

    public override void PlayerEntered()
    {
        Debug.Log("PlayerEntered exit cell");
    }
}