using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    public class CellData { public bool IsPassable; }

    [SerializeField] private Tilemap m_tilemap;
    [SerializeField] private Grid m_Grid;

    [SerializeField] private PlayerController m_PlayerController;

    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] blockingTiles;

    [SerializeField] private int width;
    [SerializeField] private int height;

    private CellData[,] m_boardCellsData;

    private void Start()
    {
        SetTiles();
    }

    private void SetTiles()
    {
        for (int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                int tileNumber = Random.Range(0, groundTiles.Length);
                m_tilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[tileNumber]);
            }
        }
    }

    // public void Init()
    // {
    //     InitializeBoardComponents();
    //
    //     int blockTileIndex = Random.Range(0, 2);
    //     SetBoardTiles(blockTileIndex);
    // }
    //
    // private void InitializeBoardComponents()
    // {
    //     m_tilemap = GetComponentInChildren<Tilemap>();
    //     m_Grid = GetComponentInChildren<Grid>();
    //     m_boardCellsData = new CellData[width, height];
    //
    //     SetMainCameraInitialPosition();
    // }
    //
    // private void SetMainCameraInitialPosition()
    // {
    //     int x = height / 2;
    //     int y = width / 2;
    //     int z = -10;
    //
    //     Camera.main.transform.position = new Vector3(x, y, z);
    // }
    //
    // private void SetBoardTiles(int wallIndexArg)
    // {
    //     for (int y = 0; y < height; ++y)
    //     {
    //         for (int x = 0; x < width; ++x)
    //         {
    //             Tile tile;
    //             m_boardCellsData[x, y] = new CellData();
    //
    //             // Wall tiles when x,y == 0 || -1
    //             if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
    //             {
    //                 tile = blockingTiles[wallIndexArg];
    //                 m_boardCellsData[x, y].IsPassable = false;
    //             }
    //             else
    //             // Inner tiles
    //             {
    //                 tile = groundTiles[Random.Range(0, groundTiles.Length)];
    //                 m_boardCellsData[x, y].IsPassable = true;
    //             }
    //
    //             m_tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    //         }
    //     }
    // }
    //
    // public Vector3 CellIndexToWorldPosition(Vector2Int cellIndex)
    // {
    //     return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    // }
    //
    // public CellData GetCellData(Vector2Int cellIndex)
    // {
    //
    //     // Check if cellIndex is out of bounds of the board
    //     // using width and height values
    //     if (IsCellOutOfBounds(cellIndex))
    //     {
    //         Debug.LogError("Cell Index out of bounds");
    //         return null;
    //     }
    //
    //     // Return cell data at x,y index
    //     return m_boardCellsData[cellIndex.x, cellIndex.y];
    //
    //     bool IsCellOutOfBounds(Vector2Int cellIndex)
    //     {
    //         return
    //         cellIndex.x < 0 ||
    //         cellIndex.y < 0 ||
    //         cellIndex.x >= width ||
    //         cellIndex.y >= height;
    //     }
    // }

}
