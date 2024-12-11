using System;
using UnityEngine;

public class EnemyCellObject : CellObject
{
    // public for testing
    public int initHealth;
    public int m_Health; 
    public bool followPlayer; 
    public Animator animator;

    private void Start()
    {
        GameManager.Instance.TurnHandler.OnTick += HandleTurn;
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);

        followPlayer = Utils.GetNewRandomInt(0, 3) == 1;
        
        initHealth = 3;
        m_Health = initHealth;
    }

    public override bool PlayerWantsToEnter()
    {
        m_Health -= 1;
        if (m_Health <= 0)
        {
            Destroy(gameObject);

            // Removing the line below makes the player
            // wait 1 tick to enter the enemy cell
            return true;
        }

        return false;
    }

    private void HandleTurn()
    {
        var playerCell = GameManager.Instance.playerController.GetCellPos();

        var xDist = playerCell.x - m_ThisCellCoord.x;
        var yDist = playerCell.y - m_ThisCellCoord.y;

        var playerAboveBelow = Mathf.Abs(yDist) == 1 && xDist == 0;
        var playerLeftRight = Mathf.Abs(xDist) == 1 && yDist == 0;
        
        // If adjacent to the player, attack
        if (playerAboveBelow || playerLeftRight)
        {
            GameManager.Instance.UpdateFood(-1);
            Debug.Log($"Text shown after player tick. " +
                      $"Player near to enemy. dist: " +
                      $"x:{xDist}, y:{yDist}");
            return;
        }
        
        if(followPlayer) TryMove(xDist, yDist);
        return;

        void TryMove(int i, int yDist1)
        {
            // If not adjacent to the player, try moving towards the player
            // if xDist is greater than yDist
            if (Mathf.Abs(i) > Mathf.Abs(yDist1))
            {
                if (!TryMoveInAxis(i, 0)) // If not moving on x-axis
                {
                    TryMoveInAxis(0, yDist1); // Then try moving in y-axis
                }
            }
            // yDist is greater than xDist
            else
            {
                if (!TryMoveInAxis(0, yDist1)) // If not moving on y-axis
                {
                    TryMoveInAxis(i, 0); // Then try moving in x-axis
                }
            }
        }
    }

    bool MoveTo(Vector2Int targetCoord)
    {
        var targetCellData = m_BoardManager.GetCellData(targetCoord);

        var cellNotPassable =
            targetCellData == null ||
            !targetCellData.IsPassable ||
            targetCellData.ContainedObject != null;

        // Early exit if target cell is invalid or blocked
        if (cellNotPassable) return false;

        // Remove the enemy cell obj from the current cell
        var currentCellData = m_BoardManager.GetCellData(m_ThisCellCoord);
        currentCellData.ContainedObject = null;

        // Assign this enemy cell obj to the new cell
        // Assign this cell coord
        // Move the enemy to the new cell
        targetCellData.ContainedObject = this;
        m_ThisCellCoord = targetCoord;
        transform.position = m_BoardManager.SetCellToWorld(targetCoord);
        return true;
    }
    
    bool TryMoveInAxis(int xDist, int yDist)
    {
        var direction = new Vector2Int();
        
        // Mathf.Sign(f) returns the sign of f --> 1, -1, 0
        direction.x = (int)Mathf.Sign(xDist); // Set the direction.x value based on xDist
        direction.y = (int)Mathf.Sign(yDist); // Set the direction.y value based on yDist

        return MoveTo(m_ThisCellCoord + direction);
    }

    public void HandleGameOver() => animator.enabled = false;

    private void OnDestroy()
    {
        GameManager.Instance.TurnHandler.OnTick -= HandleTurn;
    }
}