using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnUnit : Command
{
    public CommandSpawnUnit(SelectableObjectManager _selMng, CurrencyManager _curMng, UIManager _uiMng)
    {
        selMng = _selMng;
        curMng = _curMng;
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        EUnitType tempType = (EUnitType)_objects[0];
        if (curMng.CanSpawnUnit(tempType) && selMng.CanSpawnunit())
        {
            selMng.SpawnUnit(tempType);
            curMng.SpawnUnit(tempType);
            ArrayHUDCommand.Use(EHUDCommand.HIDE_UNIT_INFO);
            uiMng.SpawnUnit(tempType);
        }
        else
            Debug.Log("fail");
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
    private UIManager uiMng = null;
}
