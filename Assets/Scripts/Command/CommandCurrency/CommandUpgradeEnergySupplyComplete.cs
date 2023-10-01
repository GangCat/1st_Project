using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeEnergySupplyComplete : Command
{
    public CommandUpgradeEnergySupplyComplete(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        curMng.UpgradeEnergySupply();
    }

    private CurrencyManager curMng = null;
}
