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
        EUnitType tempType = (EUnitType)_objects[0];
        if (curMng.CanSpawnUnit(tempType) && selMng.CanSpawnunit())
        {
            selMng.RequestSpawnUnit(tempType);
            curMng.SpawnUnit(tempType);
        }
        else
            Debug.Log("fail");
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
}
