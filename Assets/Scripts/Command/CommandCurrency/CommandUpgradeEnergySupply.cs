using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeEnergySupply : Command
{
    public CommandUpgradeEnergySupply(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        if(curMng.CanUpgradeETC(EUpgradeETCType.ENERGY_SUPPLY))
            curMng.IncreaseEnergySupply();
    }

    private CurrencyManager curMng = null;
}
