using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_playerCell;

    private void Update()
    {
        Vector2Int newCellTarget = m_playerCell;
        var hasMoved = false;
        
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if (hasMoved) MovePlayer(newCellTarget);
    }

    private void MovePlayer(Vector2Int newCellTarget)
    {
        BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);
        if (cellData != null && cellData.IsPassable)
        {
            // Update player Vector2Int
            m_playerCell = newCellTarget;

            // Update player position with the Vector2Int newCellTarget
            transform.position = m_Board.CellToWorld(newCellTarget);
        }
    }

    // newCell is a Vector2Int, so it is required
    // To transform a cell index[,] into Vector3
    // Using the method GetCellCenterWorld() of a Grid
    public void Spawn(BoardManager boardManager, Vector2Int newCell)
    {
        m_Board = boardManager;
        m_playerCell = newCell;

        transform.position = m_Board.CellToWorld(newCell);
    }
}