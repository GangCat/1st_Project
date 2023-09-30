using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(SelectableObjectManager _selMng, CurrencyManager _curMng)
    {
        selMng = _selMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        if(curMng.SpawnUnit((ESpawnUnitType)_objects[0]))
            selMng.SpawnUnit((ESpawnUnitType)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
}
