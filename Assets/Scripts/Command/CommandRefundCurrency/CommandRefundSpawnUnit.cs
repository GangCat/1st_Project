using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundSpawnUnit : Command
{
    public CommandRefundSpawnUnit(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleSpawnUnit((EUnitType)_objects[0]);
    }

    private CurrencyManager curMng = null;
}
