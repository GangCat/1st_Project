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
        if (selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeUnit((EUnitUpgradeType)_objects[0]))
            selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
    private CurrencyManager curMng = null;
}
