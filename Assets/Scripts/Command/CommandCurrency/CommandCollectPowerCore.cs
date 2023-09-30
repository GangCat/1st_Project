using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCollectPowerCore : Command
{
    public CommandCollectPowerCore(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.IncreaseCore((uint)_objects[0]);
    }

    private CurrencyManager curMng = null;
}
