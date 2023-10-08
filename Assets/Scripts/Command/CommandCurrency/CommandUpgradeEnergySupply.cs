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
        StructureMainBase main = SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<StructureMainBase>();

        if (curMng.CanUpgradeETC(EUpgradeETCType.ENERGY_SUPPLY) && !main.IsProcessingUpgrade)
        {
            curMng.UpgradeETC(EUpgradeETCType.ENERGY_SUPPLY);
            main.UpgradeEnergySupply();
        }
    }

    private CurrencyManager curMng = null;
}
