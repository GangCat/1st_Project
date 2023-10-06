using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeUnit : Command
{
    public CommandUpgradeUnit(SelectableObjectManager _selMng, CurrencyManager _curMng)
    {
        selMng = _selMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        StructureBarrack tempBarrack = selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>();
        EUnitUpgradeType upgradeType = (EUnitUpgradeType)_objects[0];

        if (tempBarrack.CanUpgradeUnit(upgradeType) && curMng.CanUpgradeUnit(upgradeType))
        {
            curMng.UpgradeUnit(upgradeType);
            tempBarrack.UpgradeUnit(upgradeType);
        }
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
}
