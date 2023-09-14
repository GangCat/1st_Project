using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(BuildingBarrack _barrack, ESpawnUnitType _unitType)
    {
        barrack = _barrack;
        unitType = _unitType;
    }

    public override void Execute()
    {
        barrack.SpawnUnit(unitType);
    }

    private BuildingBarrack barrack = null;
    private ESpawnUnitType unitType = ESpawnUnitType.NONE;
}
