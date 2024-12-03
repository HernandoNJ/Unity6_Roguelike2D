using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // [SerializeField] private BoardManager m_board;
    // [SerializeField] private Vector2Int m_CellPosition; // It stores the current player cell index
    //
    // private void Update()
    // {
    //     Vector2Int newTargetCell = m_CellPosition;
    //     bool hasMoved = false;
    //
    //     UpdateTargetCell(ref newTargetCell, ref hasMoved);
    //
    //     if (hasMoved)
    //     {
    //         CheckCellIsPassable(newTargetCell);
    //     }
    // }
    //
    // public void SpawnPlayer(BoardManager boardManager, Vector2Int cellIndex)
    // {
    //     m_board = boardManager;
    //     MoveToNextCell(cellIndex);
    // }
    //
    // private void MoveToNextCell(Vector2Int cellIndex)
    // {
    //     m_CellPosition = cellIndex;
    //     transform.position = m_board.CellIndexToWorldPosition(m_CellPosition);
    // }
    //
    // private void CheckCellIsPassable(Vector2Int newTargetCell)
    // {
    //     BoardManager.CellData cellData = m_board.GetCellData(newTargetCell);
    //
    //     // If the cell is passable
    //     // Update Tick and move the player
    //     if (cellData != null && cellData.IsPassable)
    //     {
    //         GameManager.Instance.TurnManager.Tick();
    //         MoveToNextCell(newTargetCell);
    //     }
    // }
    //
    // private void UpdateTargetCell(ref Vector2Int newTargetCell, ref bool hasMoved)
    // {
    //     // Update targetCell if key is pressed
    //     if (Keyboard.current.upArrowKey.wasPressedThisFrame ||
    //         Keyboard.current.wKey.wasPressedThisFrame)
    //     {
    //         newTargetCell.y += 1;
    //         hasMoved = true;
    //     }
    //     else if (Keyboard.current.downArrowKey.wasPressedThisFrame ||
    //         Keyboard.current.sKey.wasPressedThisFrame)
    //     {
    //         newTargetCell.y -= 1;
    //         hasMoved = true;
    //     }
    //     else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
    //     {
    //         newTargetCell.x += 1;
    //         hasMoved = true;
    //     }
    //     else if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
    //     {
    //         newTargetCell.x -= 1;
    //         hasMoved = true;
    //     }
    // }
}
