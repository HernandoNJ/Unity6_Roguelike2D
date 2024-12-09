using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoardManager m_Board;
    [SerializeField] private Vector2Int m_playerCell;

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

    private void MovePlayer(Vector2Int newCellVector)
    {
        //check if the new position is passable, then move there if it is.
        BoardManager.CellData cellData = m_Board.GetCellData(newCellVector);
        
        if (cellData != null && cellData.IsPassable)
        {
            GameManager.Instance.TurnHandler.Tick();
            MoveTo(newCellVector);
            
            if (cellData.ContainedObject != null)
            {
                cellData.ContainedObject.PlayerEntered();
            }
        }
    }

    private void MoveTo(Vector2Int newCellTarget)
    {
        // Update player Vector2Int
        m_playerCell = newCellTarget;

        // Update player position with the Vector2Int newCellTarget
        transform.position = m_Board.SetCellToWorld(newCellTarget);
    }

    // newCell is a Vector2Int, so it is required
    // To transform a cell index[,] into Vector3
    // Using the method GetCellCenterWorld() of a Grid
    public void Spawn(BoardManager boardManager, Vector2Int newCell)
    {
        m_Board = boardManager;
        m_playerCell = newCell;

        transform.position = m_Board.SetCellToWorld(newCell);
    }
}