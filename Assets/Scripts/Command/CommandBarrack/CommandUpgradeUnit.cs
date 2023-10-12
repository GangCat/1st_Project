using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeUnit : Command
{
    public CommandUpgradeUnit(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        StructureBarrack tempBarrack = SelectableObjectManager.GetFirstSelectedObjectInList().GetComponent<StructureBarrack>();
        if (tempBarrack.IsProcessingSpawnUnit) return;

        EUnitUpgradeType upgradeType = (EUnitUpgradeType)_objects[0];
        if (tempBarrack.CanUpgradeUnit(upgradeType) && curMng.CanUpgradeUnit(upgradeType))
        {
            curMng.UpgradeUnit(upgradeType);
            tempBarrack.UpgradeUnit(upgradeType);
        }
    }

    private CurrencyManager curMng = null;
}
