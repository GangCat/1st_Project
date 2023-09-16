using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(SelectableObjectManager _selMng, ESpawnUnitType _unitType)
    {
        selMng = _selMng;
        unitType = _unitType;
    }

    public override void Execute()
    {
        selMng.SpawnUnit(unitType);
    }

    private SelectableObjectManager selMng = null;
    private ESpawnUnitType unitType = ESpawnUnitType.NONE;
}
