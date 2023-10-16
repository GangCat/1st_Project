using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundUpgradeStructure : Command
{
    public CommandRefundUpgradeStructure(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleUpgradeStructure((EObjectType)_objects[0], (int)_objects[1]);
    }

    private CurrencyManager curMng = null;
}
