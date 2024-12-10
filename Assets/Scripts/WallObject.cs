using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile obstacleTile;
    public override void Init(Vector2Int cellVector)
    {
        base.Init(cellVector);
        GameManager.Instance.boardManager.SetCelltile(cellVector,obstacleTile);
    }
}
