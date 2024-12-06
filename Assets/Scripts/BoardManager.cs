using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool IsPassable;
    }

    // Visual representation of the board.
    // Unity's Tilemap is used to draw 2D tile-based maps.
    [SerializeField] private Tilemap m_tilemap;

    // It provides the coordinate system and structure
    // For managing the tilemap and positioning objects.
    [SerializeField] private Grid m_Grid;

    [SerializeField] private PlayerController m_PlayerController;

    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;
    [SerializeField] private Tile[] blockingTiles;

    [SerializeField] private int width;
    [SerializeField] private int height;

    // Array to store whether each cell in the grid is passable
    // 2D array containing objects of type CellData
    // There will be two indices to access it
    // m_BoardData[0,0] is the first item of the first line,
    // m_BoardData[1,3] the fourth item of the second line
    private CellData[,] m_boardCellsData;

    private void Start()
    {
        m_tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        m_boardCellsData = new CellData[width, height];

        SetTiles();
        SpawnPlayer();
    }

    private void SetTiles()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;

                m_boardCellsData[x, y] = new CellData();

                var isWallTile = x == 0 || y == 0 || x == width - 1 || y == height - 1;

                // Set the tile as passable if it is a ground tile.
                // If a wall tile, set passable to false
                if (isWallTile)
                {
                    // Wall tiles
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    m_boardCellsData[x, y].IsPassable = false;
                }
                else
                {
                    // Ground tiles
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    m_boardCellsData[x, y].IsPassable = true;
                }

                m_tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        // If cellIndex is out of the grid bounds, return null
        if (cellIndex.x < 0 || cellIndex.x >= width ||
            cellIndex.y < 0 || cellIndex.y >= height)
        {
            return null;
        }

        return m_boardCellsData[cellIndex.x, cellIndex.y];
    }

    private void SpawnPlayer()
    {
        m_PlayerController.Spawn(this, new Vector2Int(1, 1));
    }

    /// <summary>
    /// Receives a cell index[,] as Vector2Int
    /// and returns a value to be used as a transform position
    /// </summary>
    /// <param name="cellIndex">Vector2Int to be transformed</param>
    /// <returns>Vector3</returns>
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
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