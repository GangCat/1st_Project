using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeUnitDmg : Command
{
    public CommandUpgradeUnitDmg(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeUnitDmg();
    }

    private SelectableObjectManager selMng = null;
}
