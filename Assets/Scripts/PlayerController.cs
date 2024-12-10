using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoardManager m_Board;
    [FormerlySerializedAs("m_PlayerCellPosition")] [SerializeField] private Vector2Int m_CellPosition;
    [SerializeField] private Vector3 m_MoveTarget;
    [SerializeField] private bool m_IsGameOver;
    [SerializeField] private bool m_IsMoving;
    [SerializeField] private float m_MoveSpeed = 5f;

    public void Init()
    {
        m_IsMoving = false;
        m_IsGameOver = false;
    }

    private void Update()
    {
        var newCellTarget = m_CellPosition;
        var hasMoved = false;

        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }

            return;
        }

        if (m_IsMoving)
        {
            // Move player towards move target which is modified in MoveTo()
            transform.position = Vector3.MoveTowards(
                transform.position,
                m_MoveTarget,
                m_MoveSpeed * Time.deltaTime);

            // player position reaches move target
            if (transform.position == m_MoveTarget)
            {
                // player is not moving
                // Set cell data
                // If cell data contains an object, call PlayerEntered()
                m_IsMoving = false;
                var cellData = m_Board.GetCellData(m_CellPosition);

                if (cellData.ContainedObject != null)
                    cellData.ContainedObject.PlayerEntered();
            }
        }
        
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

        if (hasMoved)
        {
            // Get CellData at newCellTarget 
            var cellData = m_Board.GetCellData(newCellTarget);

            // Check if the new position is passable
            // If so, Call Tick() and move player
            if (cellData != null && cellData.IsPassable)
            {
                // Tick once, including bumping into an object
                GameManager.Instance.TurnHandler.Tick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                }
            }
        }
    }

    private void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        m_IsMoving = true;

        m_MoveTarget = m_Board.SetCellToWorld(m_CellPosition);
        transform.position = m_MoveTarget;
    }

// newCell is a Vector2Int, so it is required
// To transform a cell index[,] into Vector3
// Using the method GetCellCenterWorld() of a Grid
// When calling SetCellToWorld()
    public void Spawn(BoardManager boardManager, Vector2Int newCell)
    {
        m_Board = boardManager;
        MoveTo(newCell);
    }

    public void SetGameOver() => m_IsGameOver = true;
}