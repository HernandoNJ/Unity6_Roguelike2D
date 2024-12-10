using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoardManager m_Board;
    [SerializeField] private Vector2Int m_playerCell;

    private void Update()
    {
        var newCellTarget = m_playerCell;
        var arrowKeyPressed = false;
        
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            arrowKeyPressed = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            arrowKeyPressed = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            arrowKeyPressed = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            arrowKeyPressed = true;
        }

        if (arrowKeyPressed) MovePlayer(newCellTarget);
    }

    private void MovePlayer(Vector2Int newCellVector)
    {
        // Get CellData at newCellVector 
        var cellData = m_Board.GetCellData(newCellVector);
        
        // Check if the new position is passable, then move there if it is.
        // If cell is not passable, tick is not counted
        if (cellData != null && cellData.IsPassable)
        {
            // Tick once, including bumping into an object
            GameManager.Instance.TurnHandler.Tick();
            
            // CellData doesn't contain an object, move the player
            if (cellData.ContainedObject == null)
            {
                MoveTo(newCellVector);
            }
            else if (cellData.ContainedObject.PlayerWantsToEnter())
            {
                MoveTo(newCellVector);
                
                // Call PlayerEntered AFTER moving the player!
                // Otherwise, not in cell yet
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