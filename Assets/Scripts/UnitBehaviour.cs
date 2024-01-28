using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitBehaviour : MonoBehaviour
{
    // These fields are defined in Unit Prefab.
    [SerializeField] public UnitType   unitType;
    [SerializeField] public int        attackRange;
    [SerializeField] public int        health;
    [SerializeField] public int        attackDamage;
    [SerializeField] public GameObject hpBar;
    [SerializeField] public bool       alive;

    [NonSerialized ] public GameObject attackTarget;
    [NonSerialized ] public Directions direction;       // Unit look-move direction.
    [NonSerialized ] public Cell       cell;            // Unit current position on the board.
    [NonSerialized ] public Vector3    movePosition;    // Position on which unit is moving.
    [NonSerialized ] public Team       team;            // Team.
    [NonSerialized ] public UnitStates state;           // Unit state.
    
    void UnitBehaveIdle()
    {
        // TODO: Handle better if unit is not on the board!
        if (cell.row == -1 && cell.col == -1)
        {
            return;
        }

        var path = FindShortestPath();

        // TODO: If no path stay idle. Handle not to call UnitBehaveIdle every frame - Look for board change.
        if (path.Count == 0 || path.Count == 1)
        {
            return;
        }

        // If enemy is in melee range go to attack state.
        if (path.Count == 2 && attackRange == 1)
        {
            attackTarget = BoardSystemBehaviour.boardMatrix[path[1].row, path[1].col];
            state = UnitStates.Attack;

            var animator = GetComponent<Animator>();
            animator.Play(unitType.ToString() + state.ToString() + direction.ToString());
            return;
        }

        var dir = new Vector2(path[1].col - cell.col, cell.row - path[1].row);

        direction = Utils.GetDirectionFromVector(dir);

        // TODO: Handle better offset.
        var currPos = gameObject.transform.position;
        movePosition = new Vector3(currPos.x + (dir.x * 1.25f), currPos.y + (dir.y * 1.25f), 0);

        Debug.Log(string.Format("GOTO: {0} : {1}", path[1].row, path[1].col));

        // TODO: Handle this in board system.
        BoardSystemBehaviour.MoveUnitOnBoard(gameObject, cell, path[1]);

        GetComponent<Animator>().Play(unitType.ToString() + "Move" + direction.ToString());
        state = UnitStates.Move;
    }

    void UnitBehaveMove()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, movePosition, Time.deltaTime);

        if (gameObject.transform.position == movePosition)
        {
            state = UnitStates.Idle;
        }
    }

    void UnitBehaveAttack()
    {
        
    }

    void Update()
    {
        switch (state)
        {
            case UnitStates.Idle: UnitBehaveIdle(); break;
            case UnitStates.Move: UnitBehaveMove(); break;
            case UnitStates.Attack: UnitBehaveAttack(); break;
            case UnitStates.Die: UnitBehaveDie(); break;
            default: break;
        }
    }

    public void UnitBehaveDie()
    {
  state = UnitStates.Die;

        GetComponent<Animator>().Play(unitType.ToString() + state.ToString());
    }

    private List<Cell> FindShortestPath()
    {
        var queue = new Queue<List<Cell>>();
        var seen  = new HashSet<Cell>();
        var cells = new List<Cell> { cell };

        queue.Enqueue(cells);
        seen.Add(cell);

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();

            var lastCell = path[path.Count - 1];

            // TODO: Check only for enemy team!
            var obj = BoardSystemBehaviour.boardMatrix[lastCell.row, lastCell.col];
            if (obj && obj != gameObject)
            {
                return path;
            }
            
            int[] dx = { -1,  1,  0,  0 };
            int[] dy = {  0,  0, -1,  1 };

            for (int i = 0; i < 4; i++)
            {
                int newx = lastCell.row + dx[i];
                int newy = lastCell.col + dy[i];

                if (newx >= 0 && newx < Constants.Rows && newy >= 0 && newy < Constants.Cols && BoardSystemBehaviour.boardMatrix[newx, newy] != gameObject && !seen.Contains(Cell.New(newx, newy)))
                {
                    var nlist = path.Select(c => c).ToList();
                    nlist.Add(Cell.New(newx, newy));

                    queue.Enqueue(nlist);
                    seen.Add(Cell.New(newx, newy));
                }
            }
        }
        return new List<Cell>();
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;

        float newScale = Constants.HPBarScale * health / 100;
        hpBar.transform.localScale = new Vector3(newScale, hpBar.transform.localScale.y, hpBar.transform.localScale.z);

        if (health <= 0)
        {
            Die();
            return true;
        }
        
        return false;
    }

    public void Die()
    {
        alive = false;
        state = UnitStates.Die;

        GetComponent<Animator>().Play(unitType.ToString() + state.ToString());
    }

    public void UnitFinishDeadAnimationEvent()
    {
        BoardSystemBehaviour.boardMatrix[cell.row, cell.col] = null;
        Destroy(gameObject);
    }

    public void UnitMeleeAttackConnectAnimationEvent()
    {
        // Abort if there is no target.
        if (attackTarget == null || attackTarget.GetComponent<UnitBehaviour>().alive == false)
        {
            return;
        }

        // When attack connects to its target - target takes damage.
        bool didDie = attackTarget.GetComponent<UnitBehaviour>().TakeDamage(attackDamage);
        if (didDie)
        {
            attackTarget = null;
            state = UnitStates.Idle;
        }
    }

}
