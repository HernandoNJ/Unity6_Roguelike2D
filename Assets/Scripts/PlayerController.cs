using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoardManager m_Board;
    [SerializeField] private Animator m_Animator;
    [FormerlySerializedAs("m_CellPosition")] [SerializeField] private Vector2Int m_CellPos;
    [SerializeField] private Vector3 m_MoveTarget;
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private bool m_IsMoving;
    [SerializeField] private bool m_IsGameOver;

    private void OnEnable()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Init()
    {
        m_IsGameOver = false;
        m_Animator.SetBool("Moving", false);
    }

    private void Update()
    {
        if (m_IsGameOver)
        {
            HandleGameOverInput();
            return;
        }

        PlayerSmoothMoveToTargetCell();
        HandleInput();
    }

    private void PlayerSmoothMoveToTargetCell()
    {
        if (!m_IsMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            m_MoveTarget,
            m_MoveSpeed * Time.deltaTime);

        //player reaches move target pos
        if (transform.position == m_MoveTarget)
        {
            m_Animator.SetBool("Moving", false);
            m_IsMoving = false;
            Debug.Log($"is moving: {m_IsMoving}");
        }
    }

    private void HandleGameOverInput()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            GameManager.Instance.StartNewGame();
            m_Animator.enabled = true;
        }
    }

    private void HandleInput()
    {
        // To prevent the player from moving diagonally.
        if (m_IsMoving) return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            HandleDirectionalInput("y", 1);
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            HandleDirectionalInput("y", -1);
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            HandleDirectionalInput("x", -1);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            HandleDirectionalInput("x", 1);
        else if(Keyboard.current.escapeKey.wasPressedThisFrame)
            Application.Quit();
    }

    private void HandleDirectionalInput(string axis, int increment)
    {
        var newCellTarget = m_CellPos;

        if (axis == "x") newCellTarget.x += increment;
        else if (axis == "y") newCellTarget.y += increment;

        Debug.Log($"Input detected: {newCellTarget}");
        AttemptMoveTo(newCellTarget);
    }

    private void AttemptMoveTo(Vector2Int newCell)
    {
        Debug.Log("Attempting to move to target cel");

        // Get CellData at newCellVector 
        var cellData = m_Board.GetCellData(newCell);

        // Check if the new position is passable, then move there if it is.
        // If cell is not passable, tick is not counted
        if (cellData != null && cellData.IsPassable)
        {
            // Tick once, including bumping into an object
            GameManager.Instance.TurnHandler.Tick();

            // CellData doesn't contain an object, move the player
            if (cellData.ContainedObject == null)
            {
                MoveTo(newCell);
            }
            else if (cellData.ContainedObject.PlayerWantsToEnter())
            {
                // PlayerEntered called after moving the player
                // To ensure it is in the cell
                MoveTo(newCell);
                cellData.ContainedObject.PlayerEntered();
            }
        }
    }

    private void MoveTo(Vector2Int newCellTarget)
    {
        m_IsMoving = true;
        Debug.Log($"MoveTo is moving: {m_IsMoving}");

        m_CellPos = newCellTarget;
        m_MoveTarget = m_Board.SetCellToWorld(newCellTarget);
        m_Animator.SetBool("Moving", true);
    }

    public void Spawn(BoardManager boardManager, Vector2Int newCell)
    {
        m_Board = boardManager;
        SetInitialPlayerPos(newCell);
        Init();
    }

    private void SetInitialPlayerPos(Vector2Int cell)
    {
        m_CellPos = cell;
        m_MoveTarget = m_Board.SetCellToWorld(m_CellPos);
        transform.position = m_MoveTarget;
        m_Animator.SetBool("Moving", false);
    }

    public void SetGameOver()
    {
        m_IsGameOver = true;
        m_Animator.enabled = false;
        GameManager.Instance.audioSource.Stop();
        Debug.Log("Game Over");
    }

    public Vector2Int GetCellPos() => m_CellPos;
}