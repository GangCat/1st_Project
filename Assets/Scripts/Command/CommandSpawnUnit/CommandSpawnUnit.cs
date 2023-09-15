using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(StructureBarrack _barrack, ESpawnUnitType _unitType)
    {
        barrack = _barrack;
        unitType = _unitType;
    }

    public override void Execute()
    {
        barrack.SpawnUnit(unitType);
    }

    private StructureBarrack barrack = null;
    private ESpawnUnitType unitType = ESpawnUnitType.NONE;
}
