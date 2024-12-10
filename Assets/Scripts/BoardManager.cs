using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool IsPassable;
        public CellObject ContainedObject;
    }

    // Array to store whether each cell in the grid is passable
    // 2D array containing objects of type CellData
    // There will be two indices to access it
    // m_BoardData[0,0] is the first item of the first line,
    // m_BoardData[1,3] the fourth item of the second line
    private CellData[,] m_boardCellsData;

    // Visual representation of the board.
    // Unity's Tilemap is used to draw 2D tile-based maps.
    [SerializeField] private Tilemap m_tilemap;

    // It provides the coordinate system and structure
    // For managing the tilemap and positioning objects.
    [SerializeField] private Grid m_Grid;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int xExitCoord;
    [SerializeField] private int yExitCoord;

    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] blockingTiles;

    [SerializeField] private FoodObject[] foodPrefabsArray;
    [SerializeField] private WallObject wallCellPrefab;
    [SerializeField] private ExitCellObject exitCellPrefab;

    [SerializeField] private int minFoodCount = 2;
    [SerializeField] private int maxFoodCount = 5;
    [SerializeField] private int minObstCount = 1;
    [SerializeField] private int maxObstCount = 5;
    [SerializeField] private int initFoodCount;
    [SerializeField] private int initObstCount;

    [SerializeField] private List<Vector2Int> m_EmptyCellsList;

    private void OnEnable()
    {
        m_tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
    }

    public void Init()
    {
        SetInitialValues();
        SetTiles();
        GenerateExit();
        GenerateWallObstacles();
        GenerateFood();
    }

    private void SetInitialValues()
    {
        initFoodCount = Utils.GetNewRandomInt(minFoodCount, maxFoodCount);
        initObstCount = Utils.GetNewRandomInt(minObstCount, maxObstCount);

        xExitCoord = width - 2;
        yExitCoord = height - 2;
    }

    private void SetTiles()
    {
        m_boardCellsData = new CellData[width, height];

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
                    tile = blockingTiles[Random.Range(0, blockingTiles.Length)];
                    m_boardCellsData[x, y].IsPassable = false;
                }
                else
                {
                    // Ground tiles
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    m_boardCellsData[x, y].IsPassable = true;

                    // Passable & empty cell, add it to the list!
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // Remove the starting point of the player
        var playerCell = GameManager.Instance.initPlayerPosition;
        m_EmptyCellsList.Remove(playerCell);
    }

    private void GenerateExit()
    {
        var exitCoord = new Vector2Int(xExitCoord, yExitCoord);
        AddCellObject(Instantiate(exitCellPrefab), exitCoord);
        m_EmptyCellsList.Remove(exitCoord);
    }

    private void GenerateWallObstacles()
    {
        GenerateCellObjects(initObstCount, null, wallCellPrefab);
    }

    private void GenerateFood()
    {
        GenerateCellObjects(initFoodCount, foodPrefabsArray, null);
    }

    /// <summary>
    /// Receives a cell index[,] as Vector2Int
    /// and returns a value to be used as a transform position
    /// </summary>
    /// <param name="cellIndex">Vector2Int to be transformed</param>
    /// <returns>Vector3</returns>
    public Vector3 SetCellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
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

    private void GenerateCellObjects<T>(
        int count,
        T[] prefabArray,
        T singlePrefab)
        where T : CellObject
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Utils.GetNewRandomInt(0, m_EmptyCellsList.Count);
            var cellCoord = m_EmptyCellsList[randomIndex];
            m_EmptyCellsList.RemoveAt(randomIndex);

            // As T is of type CellObject
            // prefabArray and singlePrefab must be of type CellObject
            T newPrefab;

            if (singlePrefab != null) newPrefab = singlePrefab;
            else
            {
                var randomArrayIndex = Utils.GetNewRandomInt(0, prefabArray.Length);
                newPrefab = prefabArray[randomArrayIndex];
            }

            // Instantiate and place the object
            var tObj = Instantiate(newPrefab);

            AddCellObject(tObj, cellCoord);
        }
    }

    private void AddCellObject<T>(
        T cellObj,
        Vector2Int coord)
        where T : CellObject
    {
        var data = m_boardCellsData[coord.x, coord.y];
        cellObj.transform.position = SetCellToWorld(coord);
        data.ContainedObject = cellObj;
        cellObj.Init(coord);
    }

    public void CleanBoard()
    {
        if (m_boardCellsData == null) return;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cellData = m_boardCellsData[x, y];

                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }
            }
        }

        m_tilemap.ClearAllTiles();
        m_EmptyCellsList.Clear();
    }
}