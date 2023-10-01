using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(SelectableObjectManager _selMng, CurrencyManager _curMng, PopulationManager _popMng)
    {
        selMng = _selMng;
        curMng = _curMng;
        popMng = _popMng;
    }

    public override void Execute(params object[] _objects)
    {
        ESpawnUnitType tempType = (ESpawnUnitType)_objects[0];
        if (curMng.CanSpawnUnit(tempType) && popMng.CanSpawnUnit(tempType) && selMng.CanSpawnunit())
        {
            selMng.SpawnUnit(tempType);
            curMng.SpawnUnit(tempType);
        }
        else
            Debug.Log("fail");
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
    private PopulationManager popMng = null;
}
