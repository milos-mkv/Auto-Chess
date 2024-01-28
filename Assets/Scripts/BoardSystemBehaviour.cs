using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BoardSystemState
{
    Idle,
    Play
}

public class BoardSystemBehaviour : MonoBehaviour
{
    public static GameObject[,] boardMatrix = new GameObject[6, 6];
    public static Dictionary<Team, List<GameObject>> teams = new();
    public static BoardSystemState state;

    void Start()
    {
        state = BoardSystemState.Idle;
    }

    public static void MoveUnitOnBoard(GameObject unit, Cell current, Cell next)
    {
        boardMatrix[current.row, current.col] = null;
        boardMatrix[next.row, next.col] = unit;

        unit.GetComponent<UnitBehaviour>().cell = next;
    }

    public static void StartMatch()
    {
        state = BoardSystemState.Play;
    }

    public static void StopMatch()
    {
        state = BoardSystemState.Idle;
    }

    void Update()
    {
        // DEBUG: Start match.
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartMatch();
        }

        switch (state)
        {
            case BoardSystemState.Play: UpdateBoardPlayState(); break;
            case BoardSystemState.Idle: UpdateBoardIdleState(); break;
            default: break;
        }
    }

    private void UpdateBoardPlayState()
    {
        // Update units.
        foreach (var obj in teams)
        {
            List<GameObject> units = obj.Value;
            
        }
    }

    private void UpdateBoardIdleState()
    {

    }
}
