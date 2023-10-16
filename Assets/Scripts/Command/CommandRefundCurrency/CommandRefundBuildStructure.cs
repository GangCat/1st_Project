using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundBuildStructure : Command
{
    public CommandRefundBuildStructure(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleBuildStructure((EObjectType)_objects[0]);
    }

    private CurrencyManager curMng = null;
}
