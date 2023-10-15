using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundUpgradePopulation : Command
{
    public CommandRefundUpgradePopulation(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleUpgradeETC(EUpgradeETCType.CURRENT_MAX_POPULATION);
    }

    private CurrencyManager curMng = null;
}
