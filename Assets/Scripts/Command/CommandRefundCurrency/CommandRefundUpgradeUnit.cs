using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundUpgradeUnit : Command
{
    public CommandRefundUpgradeUnit(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleUpgradeUnit((EUnitUpgradeType)_objects[0]);
    }

    private CurrencyManager curMng = null;
}
