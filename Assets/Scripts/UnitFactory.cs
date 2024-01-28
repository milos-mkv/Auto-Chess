using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPrefab
{
    public UnitType type;
    public GameObject prefab;
}

public class UnitFactory : MonoBehaviour
{
    public List<UnitPrefab> unitPrefabs;

    public GameObject GetUnitPrefab(UnitType unitType)
    {
        foreach (UnitPrefab unitPrefab in unitPrefabs)
        {
            if (unitType == unitPrefab.type)
            {
                return unitPrefab.prefab;
            }
        }
        return null;
    }

    
}
