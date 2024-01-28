using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UnitSystem : MonoBehaviour
{
    public UnitFactory unitFactory;

    public GameObject CreateUnit(UnitType unitType, Team team)
    {
        var obj = Instantiate(unitFactory.GetUnitPrefab(unitType), new Vector3(0, 0, 0), Quaternion.identity);
        var unitBehaviour = obj.GetComponent<UnitBehaviour>();

        unitBehaviour.team      = team;
        unitBehaviour.direction = Directions.Down;
        unitBehaviour.state     = UnitStates.Idle;
        unitBehaviour.alive     = true;

        return obj;
    }

    public void SetUnitOnBoard(GameObject unit, Cell cell)
    {
        if (BoardSystemBehaviour.boardMatrix[cell.row, cell.col])
        {
            return;
        }

        var unitBehaviour = unit.GetComponent<UnitBehaviour>();
        unitBehaviour.cell = cell;

        var posx = Constants.BoardStartPos[0] + (cell.col * Constants.BoardCellOffset);
        var posy = Constants.BoardStartPos[1] - (cell.row * Constants.BoardCellOffset);

        unit.transform.position = new Vector3(posx, posy, 0);

        BoardSystemBehaviour.boardMatrix[cell.row, cell.col] = unit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // var unit = CreateUnit(UnitType.Kurto, Team.Red, new Vector3(0, 0, 0));
            // SetUnitOnBoard(unit, Cell.New(0, 0));

            // var unit1 = CreateUnit(UnitType.Kurto, Team.Blue, new Vector3(0, 0, 0));
            // SetUnitOnBoard(unit1, Cell.New(5, 5));
        }
    }
}