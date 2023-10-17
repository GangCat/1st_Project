using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRefundUpgradeEnergySupply : Command
{
    public CommandRefundUpgradeEnergySupply(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.CancleUpgradeETC(EUpgradeETCType.ENERGY_SUPPLY);
    }

    private CurrencyManager curMng = null;
}
