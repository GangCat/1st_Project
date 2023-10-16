using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundSpawnNuclear : Command
{
    public CommandRefundSpawnNuclear(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleSpawnNuclear();
    }

    private CurrencyManager curMng = null;
}
