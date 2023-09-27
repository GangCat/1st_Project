using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeRangedUnitDmg : Command
{
    public CommandUpgradeRangedUnitDmg(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        if(selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeRangedUnitDmg())
            selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
}
