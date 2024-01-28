using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStates
{
    Idle,
    Move,
    Attack,
    Die,
    Debug
}

public static class Utils
{
    public static Directions GetDirectionFromVector(Vector2 vector)
    { 
        if (vector.x == -1) return Directions.Left;
        if (vector.x ==  1) return Directions.Right;
        if (vector.y == -1) return Directions.Down;
        if (vector.y ==  1) return Directions.Up;

        return Directions.Down;
    }
}

public enum Directions
{
    Left,
    Right,
    Up,
    Down
}

public enum Team
{
    Red,
    Blue
}

public enum AttackType
{
    Melee,
    Range
}

/// <summary>
/// Cell with values [ row = -1, col = -1 ] mean peace is not on the board.
/// </summary>
public struct Cell
{
    public int row;
    public int col;

    public static Cell New(int row = -1, int col = -1)
    {
        Cell cell;
        cell.row = row;
        cell.col = col;
        return cell;
    }
}

public enum UnitType
{
    Kurto
}

public static class Constants
{
    public static float HPBarScale = 0.5F;
    public static int Rows = 6;
    public static int Cols = 6;

    // TODO: DONT DO THIS LIKE THIS.
    public static float[] BoardStartPos = { -8.25F, 5.0F };
    public static float BoardCellOffset = 1.25F;
}

public enum GameScenes
{
    MainMenuScene,
    CreateGameScene
}
